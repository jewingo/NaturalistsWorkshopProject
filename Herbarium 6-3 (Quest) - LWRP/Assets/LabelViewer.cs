using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelViewer : MonoBehaviour
{

    [SerializeField]
    protected string defaultKey;

    [SerializeField]
    protected List<Label> labels;


    private GameObject defaultLabel;
    private Label currentLabel;

    //This is a really stupid workaround but I don't have time to do it better!
    private IDictionary<string, int> labelLookup = new Dictionary<string, int>();


    // Start is called before the first frame update
    void Start()
    {
        ObjectRegisterVolume.OnRegister += SetLabel;
        ObjectRegisterVolume.OnUnregister += ResetLabel;
        for(int i = 0; i < labels.Count; i++)
        {
            labelLookup.Add(labels[i].name, i);
            labels[i].label.SetActive(false);
        }
        currentLabel = labels[labelLookup[defaultKey]];
        if(currentLabel != null && currentLabel.label != null)
        {
            currentLabel.label.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetLabel(string name)
    {
        if(currentLabel != null && currentLabel.label != null)
        {
            currentLabel.label.SetActive(false);
        }
        if(labelLookup.ContainsKey(name))
        {
            currentLabel = labels[labelLookup[name]];
        }
        else
        {
            currentLabel = labels[labelLookup[defaultKey]];
        }
        currentLabel.label.SetActive(true);
    }

    void ResetLabel(string name)
    {
        if (currentLabel != null && currentLabel.label != null)
        {
            currentLabel.label.SetActive(false);
        }
        currentLabel = labels[labelLookup[defaultKey]];
        currentLabel.label.SetActive(true);
    }
}
