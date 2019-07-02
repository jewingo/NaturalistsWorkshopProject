using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class CenteringVolume : MonoBehaviour
{
    //[SerializeField]
    //protected bool centered;
    [SerializeField]
    protected int item_limit = 0;
    private Rigidbody body;
    private bool limited = false;
    [SerializeField]
    protected float breakForce = 0;
    [SerializeField]
    protected float springForce = 1;
    [SerializeField]
    protected float damping = 1;


    private List<SpringJoint> joints;
    // Start is called before the first frame update
    void Start()
    {
        Collider c = GetComponent<Collider>() ?? GetComponentInParent<Collider>();
        if (!c.isTrigger) Debug.Log("Collider of Centering Volume must be Trigger!");
        c.isTrigger = true;

        body = GetComponent<Rigidbody>();
        body.isKinematic = true;
        if (item_limit >= 0) limited = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(limited)
        {
            /*
            if(joints != null && joints.Count > item_limit)
            {
                Destroy(joints[0]);
                for(int i = 0; i < (item_limit - 1); i++)
                {
                    joints[i] = joints[i + 1];
                }
            }
            */
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>() ?? collider.gameObject.GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = rb.velocity * .1f;
            rb.angularVelocity = rb.angularVelocity * .1f;
            SpringJoint joint = collider.gameObject.GetComponent<SpringJoint>() ?? collider.gameObject.GetComponentInParent<SpringJoint>();
            print(collider.gameObject);
            //print("Parents: " + collider.gameObject.GetComponentInParent<GameObject>());
            if (joint == null) {
                joint = collider.gameObject.AddComponent<SpringJoint>();
            }

            //joints.Add(collider.gameObject.GetComponent<SpringJoint>());
            joint.connectedBody = body;
            joint.spring = springForce;
            joint.damper = damping;
            joint.maxDistance = 0;
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = new Vector3(0, 0, 0);
            joint.anchor = new Vector3(0, 0, 0);
            if(breakForce > 0) joint.breakForce = breakForce;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>() ?? other.gameObject.GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            Destroy(other.gameObject.GetComponent<SpringJoint>());
        }
    }

}
