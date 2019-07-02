using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Pullable :OVRGrabbable
{

    [SerializeField]
    protected Vector3 m_pullDirection = Vector3.forward;
    [SerializeField]
    protected float m_pullDistance = 0f;
    [SerializeField]
    protected Vector3 m_startPosition;

    private Vector3 worldStartPosition;
    private Vector3 worldDirection;

    private float m_pullMagnitude;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        worldStartPosition = transform.TransformPoint(m_startPosition);
        worldDirection = transform.TransformDirection(m_pullDirection);
    }

    public Vector3 startPosition
    {
        get { return worldStartPosition; }
    }

    public float pullDistance
    {
        get { return m_pullDistance; }
    }

    public float pullMagnitude
    {
        get { return m_pullMagnitude; }
        set { m_pullMagnitude = value; }
    }

    public Vector3 pullDirection
    {
        get { return worldDirection; }
    }


    void Awake()
    {
        if (m_startPosition == null)
        {
            m_startPosition = transform.position;
        }
        if (m_pullDistance == 0f)
        {
            m_pullDistance = m_pullDirection.magnitude;
        }
        Vector3.Normalize(m_pullDirection);

        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if (collider == null)
            {
                throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }

    }

    void Update()
    {
        /*
        if(m_grabbedBy != null)
        {
            //print(m_grabbedBy);
        }
        */
    }

    void FixedUpdate()
    {
        Color color = Color.red;
        Debug.DrawLine(worldStartPosition, worldStartPosition + worldDirection*m_pullDistance, color);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }
}
