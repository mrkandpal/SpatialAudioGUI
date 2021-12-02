using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dropzone : MonoBehaviour, IDropHandler
{
    public GameObject sourceParent;
    public int sourceCounter = 0;
    public int[] sourceActivationFlags = new int[10];

    public void OnDrop(PointerEventData eventData)
    {
        //Log drop action
        //Debug.Log("Item Dropped On " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d)
        {
            //Debug.Log(d.parentToReturnTo);

            if (sourceCounter < 4 && d.parentToReturnTo != this.transform)
            {
                d.parentToReturnTo = this.transform;
                sourceCounter++;
                this.sourceActivationFlags[d.sourceIndex - 1] = 1;
                Debug.Log(sourceCounter + " Source(s) palced in the soundfield");
                
            }
            else if(sourceCounter < 5 && d.parentToReturnTo == this.transform)
            {
                d.parentToReturnTo = this.transform;
                Debug.Log("Audio source repoositioned in the sound field");
                this.sourceActivationFlags[d.sourceIndex - 1] = 1;
            }
            else
            {
                d.parentToReturnTo = sourceParent.transform;
                Debug.Log("Sound source limit reached!");
            }
                
        }
    }

    void Start()
    {

    }

    void Update()
    {
       /* string status = "";
        for(int i = 0; i<10; i++)
        {
            if(sourceActivationFlags[i] == 1)
            {
                status = status + (i+1);
            }
        }
        Debug.Log(status);*/
    }
}
