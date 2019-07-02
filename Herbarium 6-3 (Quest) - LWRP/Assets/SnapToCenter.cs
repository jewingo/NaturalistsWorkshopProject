using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SnapToCenter : MonoBehaviour
{
    //[SerializeField]
    //protected Collider collider;
    [SerializeField]
    protected Vector3 offset = new Vector3(0, 0, 0);
    [SerializeField]
    protected Rigidbody heldBody = null;
    private Rigidbody heldCandidate = null;
    private OVRGrabbable grabbable;
    private bool heldWasKinematic = false;
    private OVRGrabbable candidateGrabbable;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (heldBody == null)
        {
            if (heldCandidate != null && !candidateGrabbable.isGrabbed)
            {
                heldBody = heldCandidate;
                heldCandidate = null;
                this.grabbable = candidateGrabbable;
                candidateGrabbable = null;
            }
        }
        if (heldBody != null)
        {
            //OVRGrabbable grabbable = heldBody.gameObject.GetComponent<OVRGrabbable>();
            if (!grabbable.isGrabbed)
            {
                heldBody.transform.position = gameObject.transform.TransformPoint(offset);
                Vector3 newRotation = gameObject.transform.InverseTransformDirection(new Vector3(0, (heldBody.transform.rotation.eulerAngles).y, 0));
                heldBody.transform.rotation = Quaternion.Euler(newRotation);
                heldBody.isKinematic = true;
            }
            else
            {
                heldCandidate = heldBody;
                candidateGrabbable = grabbable;
                releaseHeldBody();
            }
        }
    }

    private void releaseHeldBody()
    {
        //if(!grabbable.isGrabbed) heldBody.isKinematic = heldWasKinematic;
        heldBody = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (heldBody != null)
        {
            Debug.Log("Already holding rigidbody");
            return; //ignore if already holding object
        }
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return; //ignore objects with no rigidbody
        if (rb.gameObject.GetComponentInParent<OVRGrabber>() != null)
        {
            Debug.Log("Can't hold hands");
            return; //ignore hands
        }
        OVRGrabbable grabbable = rb.gameObject.GetComponent<OVRGrabbable>() ?? rb.gameObject.GetComponentInParent<OVRGrabbable>();
        if (grabbable == null)
        {
            Debug.Log("Can't hold non-grabbable objects");
            return; //ignore non-grabbable objects - may change this later to improve polymorphism
        }
        if (!grabbable.isGrabbed) {
            heldBody = rb;
            this.grabbable = grabbable;
            heldWasKinematic = rb.isKinematic;
            rb.isKinematic = true;
            heldCandidate = null;
        } else if(heldCandidate == null)
        {
            heldCandidate = rb;
            candidateGrabbable = grabbable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.attachedRigidbody == heldBody)
        {
            releaseHeldBody();
        }
        if(other.attachedRigidbody == heldCandidate)
        {
            heldCandidate = null;
            candidateGrabbable = null;
        }
    }
}
