using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upright : MonoBehaviour
{
    private OVRGrabbable parentGrab;
    private GameObject parent;
    private Vector3 angularVel;
    private Vector3 angularAccel;
    public float accelerationConstant = .05f;
    public float frictionalConstant = .1f;
    // Start is called before the first frame update
    void Start()
    {
        angularVel = new Vector3(0, 0, 0);
        parent = gameObject;
        if(GetComponent<OVRGrabbable>() != null)
        {
            parentGrab = GetComponent<OVRGrabbable>();
        }
        while(parent.transform.parent != null) {
            parent = parent.transform.parent.gameObject;
            if(parent.GetComponent<OVRGrabbable>() != null)
            {
                parentGrab = parent.GetComponent<OVRGrabbable>();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(parentGrab.isGrabbed) transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        float angleX = Mathf.LerpAngle(transform.rotation.eulerAngles.x, 0, accelerationConstant) - transform.rotation.eulerAngles.x; //calculate angular acceleration
        float angleZ = Mathf.LerpAngle(transform.rotation.eulerAngles.z, 0, accelerationConstant) - transform.rotation.eulerAngles.z;
        angularAccel = new Vector3(angleX, 0, angleZ);
        angularVel += angularAccel;                                                                                   //apply angular acceleration
        Vector3 velUnit = angularVel.normalized;
        Vector3 friction = velUnit * -1 * frictionalConstant;                                                         //calculate "friction"
        angularVel += friction;                                                                                       //apply "friction"
        if (velUnit * -1 == angularVel.normalized) angularVel = new Vector3(0, 0, 0); //zero the angular velocity if friction reverses sign
        //if (parentGrab.isGrabbed) transform.rotation = Quaternion.Euler(angleX, transform.rotation.eulerAngles.y, angleZ);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + angularVel.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + angularVel.z);
    }
}
