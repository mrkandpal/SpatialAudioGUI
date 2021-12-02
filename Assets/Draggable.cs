using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //DATA MEMBERS
    //Define dynamic parent gameObject
    public Transform parentToReturnTo = null;

    //Define identifiers for each audio source
    public string sourceName;
    public int sourceIndex;
    public bool activationStatus;

    //Define fixed initial, target and listener gameObjects
    public GameObject initialParent, targetParent, listener;

    //Define vectors to store positions of listener
    public Vector2 listenerPosition;

    //Floating point variable to store the angle between sound source and listener
    public float sourceAngle = 0.0f;

    //Vector to store direction of sound source movement
    Vector3 targetDirection;

    //UDP elements
    public int port;
    public string IP = "127.0.0.1";

    //MEMBER FUNCTIONS
    //Function to determine behaviour when game object is dragged
    public void OnDrag(PointerEventData eventData) { 
        //Log drag movement
        //Debug.Log("Dragging Object");

        //Change position of current object
        this.transform.position = eventData.position;
    }

    //Function to determine behavious when drag starts
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

    //Function to determine behaviour when drag ends
    public void OnEndDrag(PointerEventData eventData)
    {
        //Log end of drag
        //Debug.Log("Drag Ended");

        //Restore object to initial parent at the end of dragßreate 
        this.transform.SetParent(parentToReturnTo);

        //Re-enable raycasts to register drop
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set activationStatus to false before the first frame update
        this.activationStatus = false;
    }

    // Update is called once per frame
    void Update()
    {
        listenerPosition = listener.transform.up;

        //Set activationStatus to false if sound source is inside audio palette
        if (this.transform.parent == initialParent.transform)
        {
            this.activationStatus = false;
        }

        //Set activationStatus to true if sound source is inside the audio drop zone
        if(this.transform.parent == targetParent.transform)
        {
            this.activationStatus = true;
        }

        //Send UDP containing source location (angular value) to specified port if sound source is inside the audio drop zone
        if(this.activationStatus == true)
        {
            // broadcast to UDP port
            targetDirection = this.transform.position - listener.transform.position;

            this.sourceAngle = Vector3.Angle(targetDirection, listenerPosition);
            Debug.Log(this.sourceAngle);
        }
     }
}
