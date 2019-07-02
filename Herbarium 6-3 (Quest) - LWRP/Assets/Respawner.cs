using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Respawner : MonoBehaviour
{
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    protected Rigidbody body;

    // Start is called before the first frame update
    private void Awake()
    {
        spawnPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        spawnRotation = new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        //some type of "phase out" junk happens here
        //todo
        //Maybe add some "phase in" junk here
        //todo
        //place object in original location
        if (body != null)
        {
            body.isKinematic = true;
            body.velocity = Vector3.zero;
            //body.MovePosition(spawnPosition);
            //body.MoveRotation(spawnRotation);
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
            body.isKinematic = false;
            Debug.Log(gameObject.name + " respawned!");
        }
    }

    public Rigidbody rb
    {
        get { return body; }
    }
}
