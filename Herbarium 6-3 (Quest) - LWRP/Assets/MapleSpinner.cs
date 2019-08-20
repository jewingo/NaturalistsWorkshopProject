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
    private float rightingVariance;
    [SerializeField]
    private float rightingForce;
    [SerializeField]
    private float spinTorque;
    [SerializeField]
    private float spinLimit;
    private Vector3 yDiff;
    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.y < yLimit)
        {
            applyDrag();
            yDiff = Vector3.up - body.transform.up;
            if ((yDiff).magnitude > rightingVariance)
            {
                applyRightingForce();
            }
        }
    }

    void applyRightingForce()
    {
        body.AddRelativeTorque(body.transform.right * rightingForce * Mathf.Sign(yDiff.z));
        body.AddRelativeTorque(body.transform.forward * rightingForce * Mathf.Sign(yDiff.x));
    }
    void applyDrag()
    {
        body.velocity += new Vector3(0, yDrag, 0);
        body.AddTorque(Vector3.up * spinTorque, ForceMode.Acceleration);
        /*
        if (body.angularVelocity.y > spinLimit)
        {
            body.angularVelocity = new Vector3(body.angularVelocity.x, spinLimit, body.angularVelocity.z);
        }
        */
    }
}
