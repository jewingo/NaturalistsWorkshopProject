using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class OVRPullable : OVRGrabbable
{
    new OVRGrabberPuller m_pulledBy = null;


    [SerializeField]
    protected Vector3 m_pullDirection = Vector3.forward;
    [SerializeField]
    protected float m_pullDistance = 0f;
    [SerializeField]
    protected Vector3 m_startPosition;


    new public OVRGrabberPuller grabbedBy
    {
        get { return m_pulledBy;  }
    }

    public Vector3 startPosition
    {
        get { return m_startPosition; }
    }

    public float pullDistance
    {
        get { return m_pullDistance; }
    }

    public Vector3 pullDirection
    {
        get { return m_pullDirection; }
    }
    virtual public void PullBegin(OVRGrabberPuller hand, Collider grabPoint)
    {
        m_pulledBy = hand;
        m_grabbedCollider = grabPoint;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    virtual public void PullEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;
        rb.velocity = linearVelocity;
        //rb.angularVelocity = angularVelocity;
        this.m_pulledBy = null;
        m_grabbedCollider = null;
    }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    void Awake()
    {
  
        if (startPosition == null)
        {
            m_startPosition = transform.position;
        }
        if (m_grabPoints.Length == 0)
        {
            //Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if(collider == null)
            {
                throw new ArgumentException("Pullables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            //Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }
        if(pullDistance == 0f)
        {
                m_pullDistance = pullDirection.magnitude;
        }
        Vector3.Normalize(pullDirection);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
