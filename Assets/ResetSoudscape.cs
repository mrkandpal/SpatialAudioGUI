using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetSoudscape : MonoBehaviour
{
    public Button resetButton;
    public GameObject[] monoSources;
    public GameObject sourceParent;
    public GameObject audioDropZone;

    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(resetAllSources);
    }
    
    void resetAllSources()
    {
        Debug.Log("Reset Button Pressed");
        foreach (GameObject soundSource in monoSources)
        {
            soundSource.transform.SetParent(sourceParent.transform);
        }

        Dropzone dropped = audioDropZone.GetComponent<Dropzone>();
        dropped.sourceCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
