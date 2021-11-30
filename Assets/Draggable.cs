using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    

    public void OnDrag(PointerEventData eventData) { 
        //Log drag movement
        //Debug.Log("Dragging Object");

        //Change position of current object
        this.transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Log start of drag
        //Debug.Log("Drag Started");

        //Store current parent
        parentToReturnTo = this.transform.parent;

        //De-parent current object when it is being dragged
        this.transform.SetParent(this.transform.parent.parent);

        //Block raycasts to detect transitions
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Log end of drag
        //Debug.Log("Drag Ended");

        //Restore object to initial parent at the end of dragßreate 
        this.transform.SetParent(parentToReturnTo);

        //Re-enable raycasts to register drop
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
