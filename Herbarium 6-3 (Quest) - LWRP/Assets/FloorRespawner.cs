using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRespawner : MonoBehaviour
{

    //protected List<GameObject> floorItems = new List<GameObject>();
    protected Dictionary<GameObject, Respawner> respawnableObjects = new Dictionary<GameObject, Respawner>();
    protected Dictionary<GameObject, Respawner> floorItems = new Dictionary<GameObject, Respawner>();
    [SerializeField]
    protected float minimumUnspawnVelocity = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Respawner[] respawnables = FindObjectsOfType<Respawner>();
        Rigidbody tempBody;
        for(int i = 0; i < respawnables.Length; i++)
        {
            {
                respawnableObjects.Add(respawnables[i].gameObject, respawnables[i]);
                //Debug.Log(respawnables[i].gameObject.name + " added to (respawnableobjects)!");
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (floorItems.Count > 0)
        {
            List<KeyValuePair<GameObject, Respawner>> respawned = new List<KeyValuePair<GameObject, Respawner>>();
            foreach (KeyValuePair<GameObject, Respawner> rsPair in floorItems)
            {
                if (rsPair.Value.rb.IsSleeping() || !rsPair.Value.rb.isKinematic && rsPair.Value.rb.velocity.magnitude < minimumUnspawnVelocity)
                //if (rsPair.Value.rb.IsSleeping())
                //{
                    //rsPair.Value.Respawn();
                    //floorItems.Remove(rsPair.Key);
                    respawned.Add(rsPair);
                    //Debug.Log(rsPair.Key.name + " ready to respawn...");
                //} else
                {
                    //Debug.Log(rsPair.Key.name + " velocity (magnitude): " + rsPair.Value.rb.velocity.magnitude);
                }
            }
            foreach (KeyValuePair<GameObject, Respawner> rsPair in respawned)
            {
                rsPair.Value.Respawn();
                floorItems.Remove(rsPair.Key);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(respawnableObjects.ContainsKey(collision.gameObject) && !floorItems.ContainsKey(collision.gameObject)) {
            Respawner respawner;
            respawnableObjects.TryGetValue(collision.gameObject, out respawner);
            if(respawner != null)
            {
                floorItems.Add(collision.gameObject, respawner);
                //Debug.Log(collision.gameObject.name + " added to (FloorObjects)!");
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(respawnableObjects.ContainsKey(collision.gameObject) && floorItems.ContainsKey(collision.gameObject))
        {
            floorItems.Remove(collision.gameObject);
            //Debug.Log(collision.gameObject.name + " removed from (FloorObjects)!");
        }
    }
}
