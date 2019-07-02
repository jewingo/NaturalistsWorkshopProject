using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneAssetSet
{
    public string name;
    public Material skybox;
    public Light dirLight;
    public GameObject[] objects;
    public string layer;
}
