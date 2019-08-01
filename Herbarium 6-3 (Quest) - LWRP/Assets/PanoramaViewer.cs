using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanoramaViewer : MonoBehaviour
{

    [SerializeField]
    protected Material skybox;
    protected Material defaultSkybox;
    [SerializeField]
    protected Material inactiveMaterial;
    [SerializeField]
    protected GameObject inactivitySymbol;
    [SerializeField]
    protected Material preview;
    [SerializeField]
    protected Camera mainCamera;
    protected Collider boundingCollider;
    private bool wasViewing = false;
    private LayerMask defaultLayerMask;
    [SerializeField]
    protected List<PanosphereView> views;
    [SerializeField]
    protected string defaultKey;
    [SerializeField]
    protected ObjectRegisterVolume registerVolume;
    private bool regCheck = false;
    private List<string> viewLookup = new List<string>();
    private Material currentMaterial;
    private MeshRenderer renderer;
    private bool viewable = true;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        if (preview == null) { if (GetComponent<Material>() != null) preview = GetComponent<Material>(); }
        if (inactiveMaterial == null) inactiveMaterial = preview;
        if (inactivitySymbol != null)
        {
            inactivitySymbol.SetActive(false);
        }
        if (registerVolume != null)
        {
            regCheck = true;
        }
        if (skybox == null)
        {
            skybox = new Material(Shader.Find("Skybox/Cubemap"));
            skybox.mainTexture = preview.mainTexture;
        }
        
        defaultSkybox = RenderSettings.skybox;
        boundingCollider = GetComponent<Collider>();
        defaultLayerMask = mainCamera.cullingMask;
        ObjectRegisterVolume.OnRegister += SetView;
        for(int i = 0; i < views.Count; i++)
        {
            viewLookup.Add(views[i].name);
        }
        currentMaterial = this.GetComponent<Material>();
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (boundingCollider != null)
        {
            if (boundingCollider.bounds.Contains(mainCamera.transform.position))
            {
                if (!wasViewing)
                {
                    Debug.Log("Cull Everything, Replace Skybox");
                    CastPreviewToSkybox();
                    wasViewing = true;
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (boundingCollider != null)
        {
            if (!boundingCollider.bounds.Contains(mainCamera.transform.position))
            {
                if (wasViewing)
                {
                    Debug.Log("Render everything, Replace Default Skybox");
                    ResetSkybox();
                }
                wasViewing = false;
            }
        }
    }
    */

    // Update is called once per frame

    void Update()
    {
       if(boundingCollider != null)
        {
            if(boundingCollider.bounds.Contains(mainCamera.transform.position) && viewable)
            {
                if(!wasViewing)
                {
                    Debug.Log("Cull Everything, Replace Skybox");
                    CastPreviewToSkybox();
                    wasViewing = true;
                }
            }
            else
            {
                if(wasViewing)
                {
                    Debug.Log("Render everything, Replace Default Skybox");
                    ResetSkybox();
                }
                wasViewing = false;
            }
        }
    }

    void SetSkybox(Texture sky)
    {
        preview.mainTexture = sky;
        skybox.mainTexture = sky;
    }


    void SetView(string name)
    {
        Debug.Log("Object Registered: " + name);
        if (viewLookup.Contains(name))
        {
            skybox = views[viewLookup.IndexOf(name)].skybox;
            preview = views[viewLookup.IndexOf(name)].preview;
            currentMaterial = preview;
            renderer.material = currentMaterial;
            viewable = true;
            if (inactivitySymbol != null)
            {
                inactivitySymbol.SetActive(false);
            }
        } else
        {
            currentMaterial = inactiveMaterial;
            renderer.material = currentMaterial;
            viewable = false;
            if (inactivitySymbol != null)
            {
                inactivitySymbol.SetActive(true);
            }
        }
    }

    //void ResetView(string name)
    //{
    //    viewable = false;
    //}

    void CastPreviewToSkybox()
    {
        RenderSettings.skybox = skybox;
        mainCamera.cullingMask = new LayerMask();
    }
    void ResetSkybox()
    {
        RenderSettings.skybox = defaultSkybox;
        mainCamera.cullingMask = defaultLayerMask;
    }
}
