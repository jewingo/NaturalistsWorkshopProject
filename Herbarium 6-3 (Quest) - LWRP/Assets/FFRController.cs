using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFRController : MonoBehaviour
{
    private void Awake()
    {
        OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.Medium;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
