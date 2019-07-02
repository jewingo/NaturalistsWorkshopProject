using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawer : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 direction;
    public Vector3 endPosition;
    public float length = 1.0f;
    private Rigidbody body;
    //public GameObject leftHand;
    //public GameObject rightHand;

    public Vector3 currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();
        Vector3 worldScale = this.gameObject.GetComponentInParent<Transform>().lossyScale;
        print(this.gameObject.GetComponentInParent<Transform>().lossyScale.x);


        if (startPosition == Vector3.zero)
        {
            startPosition = this.gameObject.transform.localPosition;
        }
        if(direction == Vector3.zero)
        {
            direction = this.gameObject.transform.localRotation.eulerAngles;
        }
        if(endPosition == Vector3.zero)
        {
            endPosition = new Vector3(
                                        startPosition.x + length/worldScale.x*Mathf.Cos(direction.x),
                                        startPosition.y + length / worldScale.y * Mathf.Sin(direction.y), 
                                        startPosition.z + length / worldScale.z * Mathf.Sin(direction.z));
        }
        currentPosition = startPosition;
        body.MovePosition(currentPosition);

        //if(leftHand.GetComponent<OVRGrabber>() == null)
        //{
        //    print("Grabber for Left Hand not found");
        //}

        //if (rightHand.GetComponent<OVRGrabber>() == null)
        //{
        //    print("Grabber for Right Hand not found");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(currentPosition, endPosition) > 0.0f)
        {
            currentPosition = Vector3.Lerp(currentPosition, endPosition, 0.01f);
            //this.gameObject.transform.localPosition = currentPosition;
            this.gameObject.transform.localPosition = currentPosition;
        }
    }

    private void FixedUpdate()
    {
        //todo
    }
}
