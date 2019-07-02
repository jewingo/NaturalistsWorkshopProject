using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberPuller : OVRGrabber
{
    // Start is called before the first frame update
    /*override protected void Start()
    {
        base.Start();
    }
    */
    // Update is called once per frame
    /*void Update()
    {
    }
    */

    protected override void OnUpdatedAnchors()
    {
        base.OnUpdatedAnchors();
    }

    protected override void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
    {
        //base.MoveGrabbedObject(pos, rot, forceTeleport);
        //Note: pos and rot are world-scale values for grabber/puller (hand)
                //railstart is world-scale point value for pullable
                //railDirection is world-scale directional value for pullable
        if (m_grabbedObj != null && m_grabbedObj is Pullable)
        {
            Pullable pulledObj = (Pullable)m_grabbedObj;

            Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;
            Vector3 railStart = pulledObj.startPosition;
            Vector3 railDirection = pulledObj.pullDirection;
            Vector3 grabberDelta = (pos + rot * m_grabbedObjectPosOff) - railStart;
            Debug.DrawLine(railStart, railStart + grabberDelta, Color.blue);

            Vector3 posFromStart = Vector3.Project((grabberDelta), railDirection);
            Debug.DrawLine(railStart, railStart + posFromStart, Color.green);

            pulledObj.pullMagnitude = posFromStart.magnitude;
            if (pulledObj.pullMagnitude > pulledObj.pullDistance)
            {
                pulledObj.pullMagnitude = pulledObj.pullDistance;
            }
            if(Vector3.Distance(grabberDelta, railDirection*pulledObj.pullMagnitude) > (grabberDelta.magnitude))
            {
                pulledObj.pullMagnitude = 0;
            }

            //Vector3 grabbablePosition = pos + rot * m_grabbedObjectPosOff;
            //Vector3 grabbablePosition = railStart + posFromStart;
            Vector3 grabbablePosition = railStart + railDirection*pulledObj.pullMagnitude;
            //print("Magnitude: " + pulledObj.pullMagnitude);
            //print("Max Distance: " + pulledObj.pullDistance);

            if (forceTeleport)
            {
                grabbedRigidbody.transform.position = grabbablePosition;
            }
            else
            {
                grabbedRigidbody.MovePosition(grabbablePosition);
            }
        } else
        {
            base.MoveGrabbedObject(pos, rot, forceTeleport);
        }
    }
}
