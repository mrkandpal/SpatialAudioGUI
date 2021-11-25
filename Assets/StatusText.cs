using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{
    Text status;
    public GameObject audioDropZone;
    
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Dropzone dropped = audioDropZone.GetComponent<Dropzone>();
        status.text = dropped.sourceCounter + "/4 Sound Sources Selected";
    }
}
