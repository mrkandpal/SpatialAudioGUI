using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReturnZone : MonoBehaviour, IDropHandler
{
    public GameObject sourceParent;
    public GameObject audioDropZone;
    public GameObject listener;

    public void OnDrop(PointerEventData eventData)
    {
        //Log Drop Action
        //Debug.Log("Item Dropped in Return Zone");

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        Dropzone dropped = audioDropZone.GetComponent<Dropzone>();

        if (d)
        {
            //Debug.Log(d.parentToReturnTo.childCount);

            if(dropped.sourceCounter > 0 && dropped.sourceCounter <= 4) {
                if (listener.transform.IsChildOf(d.parentToReturnTo.transform))
                {
                    d.parentToReturnTo = sourceParent.transform;
                    dropped.sourceActivationFlags[d.sourceIndex - 1] = 0;
                    dropped.sourceCounter--;
                    //Debug.Log("Sound source removed" + "\n" + dropped.sourceCounter + " Source(s) active in the soundfield");
                }
            }
            else
            {
                d.parentToReturnTo = sourceParent.transform;
                //Debug.Log("Executing Here");
            }            
        }

    }
}


