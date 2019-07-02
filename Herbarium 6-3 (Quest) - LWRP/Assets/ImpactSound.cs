using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    //public float volume = 1.0f;
    //public float magnitude = 1.0f;
    public float triggerMag = 1;
    public float volumeMod = 20;
    new public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= triggerMag)
        {
            audio.volume = collision.relativeVelocity.magnitude / volumeMod;
            audio.Play();
        }
    }
}
