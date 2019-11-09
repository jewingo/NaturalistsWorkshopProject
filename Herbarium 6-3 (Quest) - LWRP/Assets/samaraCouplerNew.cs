using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class samaraCouplerNew : MonoBehaviour
{
    [SerializeField]
    private Rigidbody secondSamara;
    [SerializeField]
    private float breakDistance = 0.1f;

    [SerializeField]
    private MeshRenderer connectedMesh;
    [SerializeField]
    private Mesh mainSamaraMesh;

    private MeshFilter renderedMesh;
    private Mesh defaultMesh;

    [SerializeField]
    private MeshRenderer secondSamaraMesh;

    private MeshRenderer renderer;
    private MeshRenderer otherRenderer;

    private Rigidbody mainSamara;
    private Rigidbody otherBody;
    private OVRGrabbable mainGrab;
    private OVRGrabbable secondGrab;
    private Vector3 defaultDistance;
    private float distance;
    private bool connected = true;
    private FixedJoint joint;

    private Quaternion[] defaultOrientations;
    private MapleSpinner[] spinners;

    [SerializeField]
    private Mesh dummyMesh;

    [SerializeField]
    public bool Parent = false;

    [SerializeField]
    private bool attached = true;
    private enum Samara { left, right };

    [SerializeField]
    private Samara side = Samara.left;

    public GameObject otherSamara;
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

        renderedMesh = gameObject.GetComponent<MeshFilter>();
        defaultMesh = renderedMesh.mesh;
        

        

    }
    void Start()
    {
        mainGrab = gameObject.GetComponent<OVRGrabbable>();
        secondGrab = otherSamara.gameObject.GetComponent<OVRGrabbable>();
        //mainSamaraMesh.enabled = false;
        renderedMesh.mesh = dummyMesh;
        //secondSamaraMesh.enabled = false;
        connected = true;

        if (Parent)
        {
            otherSamara.gameObject.transform.parent = gameObject.transform;
            otherBody = otherSamara.gameObject.GetComponent<Rigidbody>();
            otherBody.isKinematic = true;
            otherSamara.gameObject.GetComponent <samaraCouplerNew>().Parent = false;
            otherRenderer = otherSamara.gameObject.GetComponent<MeshRenderer>();
            otherRenderer.enabled = false;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Parent)
        {
            distance = (mainSamara.transform.position - otherSamara.transform.position).magnitude;
            if (mainGrab.isGrabbed && secondGrab.isGrabbed)
            {
                if (distance > (defaultDistance.magnitude + breakDistance) || distance < (defaultDistance.magnitude - breakDistance))
                {
                    Uncouple();
                }
            }

            //if (joint == null && connected == true)
            //{
            //    Uncouple();
            //}

            if (!connectedMesh.enabled && connected == true)
            {
                connectedMesh.enabled = true;
            }
        }
    }

    void Uncouple()
    {
        //Destroy(joint);
        otherSamara.transform.parent = null;
        otherBody.isKinematic = false;
        connected = false;
        spinners[0].enabled = true;
        spinners[1].enabled = true;
        renderedMesh.mesh = defaultMesh;
        //connectedMesh.enabled = false;
        //mainSamaraMesh.enabled = true;
        //econdSamaraMesh.enabled = true;
    }
    void Recouple()
    {
        Quaternion tempRot = new Quaternion(mainSamara.rotation.x, mainSamara.rotation.y, mainSamara.rotation.z, mainSamara.rotation.w);
        mainSamara.isKinematic = true;
        //secondSamara.isKinematic = true;
        otherBody.isKinematic = true;
        otherBody.velocity = Vector3.zero;
        mainSamara.velocity = Vector3.zero;
        mainSamara.rotation = defaultOrientations[0];
        otherBody.rotation = defaultOrientations[1];
        otherBody.position = mainSamara.position + defaultDistance;
        mainSamara.transform.parent.rotation *= tempRot;
        //joint = gameObject.AddComponent<FixedJoint>();
        otherBody.transform.parent = mainSamara.transform;
        //joint.connectedBody = secondSamara;
        //joint.breakForce = float.PositiveInfinity;
        //joint.breakTorque = float.PositiveInfinity;
        spinners[0].enabled = false;
        spinners[1].enabled = false;
        mainSamara.isKinematic = false;
        renderedMesh.mesh = dummyMesh;
    }
}
