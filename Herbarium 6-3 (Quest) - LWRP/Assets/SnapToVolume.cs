using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SnapToVolume : MonoBehaviour
{

    //public Collider collider;
    //private Dictionary<int, bool> kinematicRecord;
    private Dictionary<Rigidbody, bool> bodies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.GetComponent<GameObject>() ?? other.GetComponentInParent<GameObject>();
        if (obj == null) return;
        Rigidbody body = obj.GetComponent<Rigidbody>();
        if (body == null) return;
        bool isKinematic = false;
        if (!bodies.TryGetValue(body, out isKinematic)) bodies[body] = isKinematic;
        body.isKinematic = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody body = other.GetComponent<Rigidbody>() ?? other.GetComponentInParent<Rigidbody>();
        if (body == null) return;

        bool isKinematic = false;
        if(bodies.TryGetValue(body, out isKinematic))
        {
            body.isKinematic = isKinematic;
            bodies.Remove(body);
        }
        
    }
}
