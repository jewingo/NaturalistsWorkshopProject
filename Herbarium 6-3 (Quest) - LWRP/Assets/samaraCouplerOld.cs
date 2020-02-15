using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class samaraCouplerOld : MonoBehaviour
{
    [SerializeField]
    private Rigidbody secondSamara;
    [SerializeField]
    private float breakDistance = 0.1f;

    [SerializeField]
    private MeshRenderer connectedMesh;
    [SerializeField]
    private MeshRenderer mainSamaraMesh;
    [SerializeField]
    private MeshRenderer secondSamaraMesh;


    private Rigidbody mainSamara;
    private OVRGrabbable mainGrab;
    private OVRGrabbable secondGrab;
    private Vector3 defaultDistance;
    private float distance;
    private bool connected = true;
    private FixedJoint joint;

    private Quaternion[] defaultOrientations;
    private MapleSpinner[] spinners;
    // Start is called before the first frame update
    private void Awake()
    {

        joint = gameObject.GetComponent<FixedJoint>();
        if (joint == null && connected == true)
        {
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = secondSamara;
        }
        joint.breakForce = float.PositiveInfinity;
        joint.breakTorque = float.PositiveInfinity;
        mainSamara = gameObject.GetComponent<Rigidbody>();
        defaultDistance = (mainSamara.transform.position - secondSamara.transform.position);
        defaultOrientations = new Quaternion[2];
        defaultOrientations[0] = mainSamara.rotation;
        defaultOrientations[1] = secondSamara.rotation;
        spinners = new MapleSpinner[2];
        spinners[0] = mainSamara.gameObject.GetComponent<MapleSpinner>();
        spinners[1] = secondSamara.gameObject.GetComponent<MapleSpinner>();
        spinners[0].enabled = false;
        spinners[1].enabled = false;

    }
    void Start()
    {
        mainGrab = gameObject.GetComponent<OVRGrabbable>();
        secondGrab = gameObject.GetComponent<OVRGrabbable>();
        mainSamaraMesh.enabled = false;
        secondSamaraMesh.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = (mainSamara.transform.position - secondSamara.transform.position).magnitude;
        if (mainGrab.isGrabbed && secondGrab.isGrabbed)
        {
            if (distance > (defaultDistance.magnitude + breakDistance) || distance < (defaultDistance.magnitude - breakDistance))
            {
                Uncouple();
            }
        }

        if (joint == null && connected == true)
        {
            Uncouple();
        }
        
        if(!connectedMesh.enabled && connected == true)
        {
            connectedMesh.enabled = true;
        }
    }

    void Uncouple()
    {
        Destroy(joint);
        connected = false;
        spinners[0].enabled = true;
        spinners[1].enabled = true;
        connectedMesh.enabled = false;
        mainSamaraMesh.enabled = true;
        secondSamaraMesh.enabled = true;
    }
    void Recouple()
    {
        Quaternion tempRot = new Quaternion(mainSamara.rotation.x, mainSamara.rotation.y, mainSamara.rotation.z, mainSamara.rotation.w);
        mainSamara.isKinematic = true;
        secondSamara.isKinematic = true;
        mainSamara.rotation = defaultOrientations[0];
        secondSamara.rotation = defaultOrientations[1];
        secondSamara.position = mainSamara.position + defaultDistance;
        mainSamara.transform.parent.rotation *= tempRot;
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = secondSamara;
        joint.breakForce = float.PositiveInfinity;
        joint.breakTorque = float.PositiveInfinity;
        spinners[0].enabled = false;
        spinners[1].enabled = false;
        mainSamara.isKinematic = false;
        secondSamara.isKinematic = false;
    }
}
