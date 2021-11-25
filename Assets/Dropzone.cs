using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dropzone : MonoBehaviour, IDropHandler
{
    public GameObject sourceParent;
    public int sourceCounter = 0;

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
                Debug.Log(sourceCounter + " Source(s) palced in the soundfield");
                
            }
            else if(sourceCounter < 5 && d.parentToReturnTo == this.transform)
            {
                d.parentToReturnTo = this.transform;
                Debug.Log("Audio source repoositioned in the sound field");
            }
            else
            {
                d.parentToReturnTo = sourceParent.transform;
                Debug.Log("Sound source limit reached!");
            }
                
        }
    }
}
