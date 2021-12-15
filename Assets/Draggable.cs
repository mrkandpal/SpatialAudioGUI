using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using UnityEngine.UI;


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

    //Define gameObjects to check for left and right limit
    public GameObject leftEnd, rightEnd;

    //Define vectors to store positions of listener
    public Vector2 listenerPosition;

    //Floating point variable to store the angle between sound source and listener
    public float sourceAngle = 0.0f;

    //Floating point variable to measure distance between sound source and listener.
    //This will be used to decide a distance-based gain for the sound sources.
    public float distanceGain = 1.0f;

    //Vector to store direction of sound source movement
    Vector3 targetDirection;

    //UDP elements
    public int port;
    public string IP = "127.0.0.1";

    //Connection elements
    IPEndPoint remoteEndPoint;
    UdpClient client;

    //String containing acontrol message
    public string statusMessage = "";

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

    //DRIVER FUNCTIONS

    //Call from shelll (as a program)
    private static void Main()
    {
        Draggable sendOBJ = new Draggable();
        sendOBJ.init();
    }

    //init()
    public void init()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(this.IP), this.port);
        client = new UdpClient();
    }

    //Send String
    private void SendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }catch (Exception err)
        {
            Debug.Log(err.ToString());
            //Debug.Log("error here");
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set activationStatus to false before the first frame update
        this.activationStatus = false;
        init();
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
        if (this.activationStatus == true)
        {
            //Calculate directionof movement
            targetDirection = this.transform.position - listener.transform.position;

            //Calculate net angle between sound source and listener
            this.sourceAngle = Vector3.Angle(targetDirection, listenerPosition);

            //If the sound source is closer to the right end, subtract the angle value from 360
            if (this.sourceAngle > 0 && Vector2.Distance(this.transform.position, leftEnd.transform.position) > Vector2.Distance(this.transform.position, rightEnd.transform.position))
            {
                this.sourceAngle = 360.0f - this.sourceAngle;
            }

            //Round off value of soureAngle to get cleaner values
            this.sourceAngle = Mathf.Floor(this.sourceAngle);

            //Calculate distance between listener and sound source to apply distance-based scalar gain
            this.distanceGain = Vector2.Distance(this.transform.position, listener.transform.position);
            this.distanceGain = Mathf.Floor(this.distanceGain);

            //Broadcast angle + gain factor over udp
            //message format - angle|gain. use '|' as delimiter

            this.statusMessage = this.sourceAngle + "|" +  this.distanceGain + "|" + this.sourceIndex;
            //Debug.Log(this.statusMessage);
            SendString(this.statusMessage);
        }
     }
}
