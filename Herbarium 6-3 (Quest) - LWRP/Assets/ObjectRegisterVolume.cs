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

    public delegate void UnregisterObject(string name);
    public static event UnregisterObject OnUnregister;
    //private Stack objectRegistry;
    private IDictionary<GameObject, OVRGrabbable> registryCandidates;
    //private List<OVRGrabbable> registryCandidates;
    private GameObject registeredObject;
    private Respawner registeredObjectRespawner;
    private Rigidbody registryCandidateBody;

    // Start is called before the first frame update
    void Start()
    {
        //objectRegistry = new Stack();
        registryCandidates = new Dictionary<GameObject, OVRGrabbable>();
        //registryCandidates = new List<OVRGrabbable>();
        registeredObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(registryCandidates.Count > 0)
        {
           //List<GameObject> items
            foreach (KeyValuePair<GameObject, OVRGrabbable> entry in registryCandidates)
            {
                //if
                if(!entry.Value.isGrabbed)
                {

                    //registryCandidates.Remove(entry.Key);
                    if (registeredObjectRespawner != null && registeredObject != entry.Key)
                    {
                        Debug.Log("Respawning " + registeredObjectRespawner.gameObject.name);
                        registeredObjectRespawner.Respawn();
                    }
                    registeredObject = entry.Key;
                    registeredObjectRespawner = registeredObject.GetComponent<Respawner>();
                    if (OnRegister != null)
                    {
                        OnRegister(registeredObject.name);
                    }
                    label.text = registeredObject.name;
                }
            }
            if(registeredObject != null)
            {
                if (registryCandidates.ContainsKey(registeredObject))
                {
                    registryCandidates.Remove(registeredObject);
                }
            }

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.layer == LayerMask.NameToLayer("VR Controller") && obj == registeredObject) return;
        if (obj != null && obj.transform != null && obj.transform.parent != null)
        {
            while (obj.transform.parent != null)
            {
                if (obj.transform.parent.gameObject.GetComponent<Collider>() == null && obj.transform.parent.name != "extra colliders" && obj.transform.parent.GetComponent<Rigidbody>() == null) break;
                obj = obj.transform.parent.gameObject;
                
            }            
        }
        //if (OnRegister != null)
        //{
        //    OnRegister(obj.name);
        //}
        if(!registryCandidates.ContainsKey(obj))
        {
            OVRGrabbable grabbable = obj.GetComponent<OVRGrabbable>();
            if(grabbable != null)
            {
                registryCandidates.Add(obj, grabbable);
                Debug.Log("Added register candidate " + obj.name);
            }
           
        }
        //label.text = obj.name;
    }

    private void OnTriggerExit(Collider other)
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
        if (registryCandidates.ContainsKey(obj))
        {
            registryCandidates.Remove(obj);
            Debug.Log("Removed register candidate " + obj.name);
        }

        if (registeredObject == obj)
        {
            registeredObject = null;
            registeredObjectRespawner = null;
            label.text = "Place Sample in Tray";
        }
        /*
        if (OnUnregister != null)
        {
            OnUnregister(obj.name);
        }
        */

    }
}
