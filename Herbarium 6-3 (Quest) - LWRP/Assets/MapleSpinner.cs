using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleSpinner : MonoBehaviour
{

    private Rigidbody body;
    [SerializeField]
    private float yLimit;
    [SerializeField]
    private float yDrag;
    [SerializeField]
    [Range(0f, 1.0f)]
    private float generalDragContribution;
    //[SerializeField]
    //private float rightingVariance;
    [SerializeField]
    private float rightingForce;
    [SerializeField]
    private float spinTorque;
    //[SerializeField]
    //private float spinLimit;
    [SerializeField]
    private float maxAngularVelocity;
    private Vector3 yDiff;
    [SerializeField]
    private float minDistance;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        body.maxAngularVelocity = maxAngularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        body.maxAngularVelocity = maxAngularVelocity;
        if (body.velocity.y < yLimit)
        {
            
            applyTorque();
            applyDrag();
            yDiff = Vector3.up.normalized - body.transform.up.normalized;
            //yDiff = body.transform.up - Vector3.up;
            applyRightingForce();
            /*
            if ((yDiff).magnitude > rightingVariance)
            {
                applyRightingForce();
            }
            */
        }
    }

    void applyRightingForce()
    {
        Vector3 prevDiff = yDiff;
        body.AddTorque(Vector3.right * rightingForce * yDiff.z);
        body.AddTorque(Vector3.forward * rightingForce * -yDiff.x);
        //body.AddForceAtPosition(Vector3.up * rightingForce, body.transform.position + Vector3.up + Vector3.forward + Vector3.right);
        yDiff = Vector3.up.normalized - body.transform.up.normalized;

        //if(yDiff.magnitude > prevDiff.magnitude)
        //{
        //    body.transform.rotation = Quaternion.Euler(0, body.transform.rotation.eulerAngles.y, 0);
        //}
        //yDiff = body.transform.up - Vector3.up;
        //if (yDiff.magnitude < minDistance)
        //{
        //    body.transform.rotation = Quaternion.Euler(0, body.transform.rotation.eulerAngles.y, 0);
        //}
        //body.transform.rotation = Quaternion.Euler(0, body.transform.rotation.eulerAngles.y, 0);
    }
    void applyDrag()
    {
        //body.velocity += new Vector3(0, yDrag, 0);
        //body.velocity += -body.velocity.normalized * yDrag * 1/(yDiff.magnitude + 1);
        body.velocity += -body.velocity.normalized * yDrag * Mathf.Clamp(body.angularVelocity.y, 0, float.PositiveInfinity)/maxAngularVelocity;
        body.velocity += -body.velocity.normalized * yDrag * body.angularVelocity.magnitude / maxAngularVelocity * generalDragContribution;
        //ody.AddTorque(Vector3.up * spinTorque, ForceMode.Acceleration);

        /*
        if (body.angularVelocity.y > spinLimit)
        {
            body.angularVelocity = new Vector3(body.angularVelocity.x, spinLimit, body.angularVelocity.z);
        }
        */
    }

    void applyTorque()
    {
        body.AddTorque(Vector3.up * spinTorque * 1 / (yDiff.magnitude + 1) , ForceMode.Acceleration);
        //body.rotation = Quaternion.Euler(0, body.rotation.eulerAngles.y + spinTorque, 0);
        //body.angularVelocity += new Vector3(0, spinTorque, 0);
    }
}
