using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectRegisterVolume : MonoBehaviour
{

    [SerializeField]
    protected TextMeshPro label;

    public delegate void RegisterObject(string name);
    public static event RegisterObject OnRegister;

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
        GameObject obj = other.gameObject;
        if (obj.layer == LayerMask.NameToLayer("VR Controller")) return;
        if (obj != null && obj.transform != null && obj.transform.parent != null)
        {
            while (obj.transform.parent != null)
            {
                if (obj.transform.parent.gameObject.GetComponent<Collider>() == null && obj.transform.parent.name != "extra colliders" && obj.transform.parent.GetComponent<Rigidbody>() == null) break;
                obj = obj.transform.parent.gameObject;
                
            }            
        }
        if (OnRegister != null)
        {
            OnRegister(obj.name);
        }
        label.text = obj.name;
    }
}
