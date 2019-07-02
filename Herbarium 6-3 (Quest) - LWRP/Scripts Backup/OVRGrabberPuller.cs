using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class OVRGrabberPuller : OVRGrabber
{
    protected OVRPullable m_pulledObj = null;
    protected Dictionary<OVRPullable, int> m_pullCandidates = new Dictionary<OVRPullable, int>();


    public OVRPullable pulledObject
    {
        get { return m_pulledObj; }
    }

    public void ForceRelease(OVRPullable pullable)
    {
        bool canRelease = (
            (m_pulledObj != null) &&
            (m_pulledObj == pullable)
        );
        if (canRelease)
        {
            PullEnd();
        }
    }

    protected void PullBegin()
    {
        float closestMagSq = float.MaxValue;
        OVRPullable closestPullable = null;
        Collider closestPullableCollider = null;

        //Iterate pull candidates and find the closest grabbable candidate
        foreach (OVRPullable pullable in m_pullCandidates.Keys)
        {
            bool canPull = !(pullable.isGrabbed && !pullable.allowOffhandGrab);
            if(!canPull)
            {
                continue;
            }
            
            for (int j = 0; j < pullable.grabPoints.Length; ++j)
            {
                Collider pullableCollider = pullable.grabPoints[j];
                // Store the closest pullable
                Vector3 closestPointOnbounds = pullableCollider.ClosestPointOnBounds(m_gripTransform.position);
                float pullableMagSq = (m_gripTransform.position - closestPointOnbounds).sqrMagnitude;
                if(pullableMagSq < closestMagSq)
                {
                    closestMagSq = pullableMagSq;
                    closestPullable = pullable;
                    closestPullableCollider = pullableCollider;
                }
            }
        }

        //Dissable grab volumes to prevent overlaps
        GrabVolumeEnable(false);

        if(closestPullable != null)
        {
            if(closestPullable.isGrabbed)
            {
                closestPullable.grabbedBy.OffhandGrabbed(closestPullable);
            }

            m_pulledObj = closestPullable;
            m_pulledObj.GrabBegin(this, closestPullableCollider);

            m_lastPos = transform.position;
            m_lastRot = transform.rotation;

            //Set up offsets for pulled object desired positiion relative to hand.
            //Needs to be changed - pulled objects should "snap" hands to them, rather than being "snapped" to hands
            //For now turn off snap offsets for all "pullable" objects
            if(m_pulledObj.snapPosition)
            {
                m_grabbedObjectPosOff = m_gripTransform.localPosition;
                if(m_pulledObj.snapOffset)
                {
                    Vector3 snapOffset = m_pulledObj.snapOffset.position;
                    if (m_controller == OVRInput.Controller.LTouch) snapOffset.x = -snapOffset.x;
                    m_grabbedObjectPosOff += snapOffset;
                }
            }
            else
            {
                Vector3 relPos = m_pulledObj.transform.position - transform.position;
                relPos = Quaternion.Inverse(transform.rotation) * relPos;
                m_grabbedObjectPosOff = relPos;
            }

            if (m_pulledObj.snapOrientation)
            {
                m_grabbedObjectRotOff = m_gripTransform.localRotation;
                if(m_pulledObj.snapOffset)
                {
                    m_grabbedObjectRotOff = m_pulledObj.snapOffset.rotation * m_grabbedObjectRotOff;
                }
            } else
            {
                Quaternion relOri = Quaternion.Inverse(transform.rotation) * grabbedObject.transform.rotation;
                m_grabbedObjectRotOff = relOri;
            }

            //Oculus original script leftovers:
            // Note: force teleport on grab, to avoid high-speed travel to dest which hits a lot of other objects at high
            // speed and sends them flying. The grabbed object may still teleport inside of other objects, but fixing that
            // is beyond the scope of this demo.
            MovePulledObject(m_lastPos, m_lastRot, true);
            if(m_parentHeldObject)
            {
                m_pulledObj.transform.parent = transform;
            }
        }
    }

    protected void PullEnd()
    {
        if (m_pulledObj != null)
        {
            //These calculations are currently mapped to the controller's "real" movement; we want to
            //use the "snapped-to" movement to calculate to be accurate
            OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
            OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
            localPose = localPose * offsetPose;

            OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
            Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
            Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);

            PullableRelease(linearVelocity, angularVelocity);
        }

        //Re-enable grab volumes to allow overlap events
        GrabVolumeEnable(true);
    }

    protected void PullableRelease(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        m_pulledObj.PullEnd(linearVelocity, angularVelocity);
        if (m_parentHeldObject) m_pulledObj.transform.parent = null;
        m_pulledObj = null;
    }

    protected virtual void MovePulledObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
    {
        if (m_pulledObj == null)
        {
            return;
        }

        //leftover from "grabbable"
        Rigidbody grabbedRigidbody = m_pulledObj.grabbedRigidbody;
        //Vector3 grabbablePosition = pos + rot * m_grabbedObjectPosOff;
        //Quaternion grabbableRotation = rot * m_grabbedObjectRotOff;

        //get the location of the pullable "start" on path in world space
        Vector3 pullableWorldStartPosition = m_pulledObj.transform.TransformPoint(m_pulledObj.startPosition);
        //get the vector from the "start" position to the grabber/puller hand
        Vector3 delta = pos - pullableWorldStartPosition;
        //convert pull direction to world space
        Vector3 pathWorldDirection = m_pulledObj.transform.TransformDirection(m_pulledObj.pullDirection);
        //Normalize world space direction vector
        //pathWorldDirection.Normalize();

        //calculate angle to produce magnitude of transformation along path
        float grabberAngle = Vector3.Angle(pathWorldDirection, pos);

        //calculate position on path due to grabber/puller hand position
        Vector3 pathTransform = Vector3.ClampMagnitude(m_pulledObj.pullDirection * Mathf.Cos(grabberAngle) * delta.magnitude, m_pulledObj.pullDistance);
        Vector3 onPathPos = pullableWorldStartPosition + pathTransform;

        //set position to start position if angle is negative
        if (Vector3.Distance(onPathPos, pos) > pos.magnitude)
        {
            onPathPos = pullableWorldStartPosition;
        }

        // Temp. removed code here:

        //if (Mathf.Cos(grabberAngle) * delta.magnitude <= m_pulledObj.pullDistance)
        //{
        //    onPathPos = pullableWorldStartPosition + m_pulledObj.pullDirection * Mathf.Cos(grabberAngle) * delta.magnitude;
        //}
        //else
        //{
        //    onPathPos = pullableWorldStartPosition + m_pulledObj.pullDirection * m_pulledObj.pullDistance;
        //}

        //convert to relative space? (oof there's a more effecient way to do this, look back over to reduce conversions)
        //transform.InverseTransformPoint(onPathPos);
        print("Pulling object!");
        Handles.color = Color.red;
        Handles.DrawWireCube(onPathPos, new Vector3(1f, 1f, 1f));

        if (forceTeleport)
        {
            //non-physics-based transform
            grabbedRigidbody.transform.position = onPathPos;
        }
        else
        {
            //physics-based transform
            //grabbedRigidbody.MovePosition(onPathPos);
            grabbedRigidbody.transform.position = onPathPos;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{

    //}

   protected override void OnUpdatedAnchors()
    {
        Vector3 handPos = OVRInput.GetLocalControllerPosition(m_controller);
        Quaternion handRot = OVRInput.GetLocalControllerRotation(m_controller);
        Vector3 destPos = m_parentTransform.TransformPoint(m_anchorOffsetPosition + handPos);
        Quaternion destRot = m_parentTransform.rotation * handRot * m_anchorOffsetRotation;
        GetComponent<Rigidbody>().MovePosition(destPos);
        //GetComponent<Rigidbody>().MoveRotation(destRot);
    }

    //override void OnTriggerEnter(Collider other)
    //{
        
    //

}
