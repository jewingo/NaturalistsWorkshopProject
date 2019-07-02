using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chair_controller : MonoBehaviour
{

    public Transform player;
    public float swivelSpeedMod;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            Vector3 position = this.transform.position;
            Vector3 newPosition = new Vector3(player.position.x, position.y, player.position.z);
            float rotationChange = this.transform.rotation.y - player.rotation.y;
            this.transform.position = newPosition;
            this.transform.Rotate(new Vector3(0, 0, swivelSpeedMod*rotationChange));
            //print(rotationChange);
        }
    }
}
