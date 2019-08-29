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
    [SerializeField]
    private float rightingForce;
    [SerializeField]
    private float spinTorque;
    //[SerializeField]
    //private float spinLimit;
    [SerializeField]
    private float maxAngularVelocity;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private bool doubleSided = true;
    [SerializeField]
    private bool immediateRightingEnabled = false;

    private Vector3 targetVector;
    private Vector3 yDiff;
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        body.maxAngularVelocity = maxAngularVelocity;
        targetVector = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if ((Vector3.up - body.transform.up.normalized).magnitude <= Mathf.Sqrt(2))
            targetVector = Vector3.up;
        else
            targetVector = -Vector3.up;
        yDiff = targetVector - body.transform.up.normalized;
        Debug.DrawLine(body.transform.position, body.transform.position + targetVector, Color.green);
        //Debug.DrawLine(body.transform.position + body.transform.up.normalized, (body.transform.position + body.transform.up.normalized) + targetVector * this.rightingForce * yDiff.magnitude / Mathf.Sqrt(2), Color.red);
        Debug.DrawLine(body.transform.position + body.transform.up.normalized, (body.transform.position + body.transform.up.normalized) + targetVector * this.rightingForce, Color.red);

        Debug.DrawLine(body.transform.position + body.transform.up.normalized, body.transform.position + targetVector.normalized);
        body.maxAngularVelocity = maxAngularVelocity;
        if (body.velocity.y < yLimit)
        {
            yDiff = targetVector - body.transform.up.normalized;
            //yDiff = Vector3.up.normalized - body.transform.up.normalized;
            //yDiff = body.transform.up - Vector3.up;
            applyRightingForce();
            applyTorque();
            applyDrag();
            
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
        //Vector3 prevDiff = yDiff;
        Vector3 rightingForce;
        Vector3 forcePos;
        //rightingForce = targetVector * this.rightingForce * (yDiff.magnitude / Mathf.Sqrt(2) + 1);
        rightingForce = targetVector * this.rightingForce;
        forcePos = body.transform.up;
        /*
        if (yDiff.magnitude <= Mathf.Sqrt(2) || doubleSided == false)
        {
            //rightingForce = (yDiff) * yDiff.magnitude / Mathf.Sqrt(2) * this.rightingForce;
            rightingForce = Vector3.up * this.rightingForce * yDiff.magnitude/Mathf.Sqrt(2);
            forcePos = body.transform.up;
        }
        else
        {
            Vector3 tempDiff = -Vector3.up - body.transform.up.normalized;
            // rightingForce = (tempDiff) * tempDiff.magnitude/Mathf.Sqrt(2) * this.rightingForce;
            rightingForce = -Vector3.up * this.rightingForce * tempDiff.magnitude / Mathf.Sqrt(2);
            forcePos = -body.transform.up;
        }
        */
        body.AddForceAtPosition(rightingForce, body.transform.position + forcePos);
        //body.AddForceAtPosition()
        //yDiff = body.transform.InverseTransformDirection(yDiff);
        /*if(yDiff.magnitude <= Mathf.Sqrt(2))
        {
            body.AddTorque(body.transform.right * rightingForce * -yDiff.z);
            body.AddTorque(body.transform.forward * rightingForce * yDiff.x);
        }
        else
        {
            body.AddTorque(Vector3.right * rightingForce * yDiff.z);
            body.AddTorque(Vector3.forward * rightingForce * -yDiff.x);
        }
        */
        //body.AddForceAtPosition(Vector3.up * rightingForce, body.transform.position + Vector3.up + Vector3.forward + Vector3.right);
        //yDiff = Vector3.up.normalized - body.transform.up.normalized;

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
        body.AddTorque(Vector3.up * spinTorque * 1 / (yDiff.magnitude + 1), ForceMode.Acceleration);
        /*
        if (yDiff.magnitude <= Mathf.Sqrt(2))
        {
            body.AddTorque(Vector3.up * spinTorque * 1 / (yDiff.magnitude + 1), ForceMode.Acceleration);
        }else
        {
            Vector3 tempDiff = -Vector3.up - body.transform.up.normalized;
            body.AddTorque(Vector3.up * spinTorque * 1 / (tempDiff.magnitude + 1), ForceMode.Acceleration);
        }
        //body.rotation = Quaternion.Euler(0, body.rotation.eulerAngles.y + spinTorque, 0);
        //body.angularVelocity += new Vector3(0, spinTorque, 0);
        */
    }
}
