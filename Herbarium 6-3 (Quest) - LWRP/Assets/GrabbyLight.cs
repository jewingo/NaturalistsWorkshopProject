using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbyLight : MonoBehaviour
{

    [SerializeField]
    protected Light light;
    [SerializeField]
    protected Material material;
    private OVRGrabbable grabbable;
    private bool wasGrabbed = false;
    [SerializeField]
    protected bool on = true;
    private float emissionColor;
    private Renderer rend;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        grabbable = gameObject.GetComponent<OVRGrabbable>();
        if(!grabbable)
        {
            Debug.Log("Grabby Light requires OVRGrabbable!");
        }
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        material.EnableKeyword("_EMISSION");
        if (on) { TurnOn(); } else TurnOff();
        //rend.material.shader = Shader.Find("_Emissive");
        //emissionColor = rend.material.GetColor("_EmissiveColor")
        //emissionColor = material.GetFloat("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if(wasGrabbed != grabbable.isGrabbed && grabbable.isGrabbed)
        {
            on = !on;
            if (on) { TurnOn(); } else TurnOff();
        }
        if(rb != null && !grabbable.isGrabbed && !on)
        {
            rb.isKinematic = false;
        }
        wasGrabbed = grabbable.isGrabbed;
    }

    void TurnOff()
    {
        if(light != null) light.enabled = false;
        material.DisableKeyword("_EMISSION");
        //material.SetFloat("_EmissionColor", 0x000000);
    }

    void TurnOn()
    {
        if (light != null) light.enabled = true;
        //material.SetFloat("_EmissionColor", emissionColor);
        material.EnableKeyword("_EMISSION");
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
}
