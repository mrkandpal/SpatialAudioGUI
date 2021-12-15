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

public class ResetSoudscape : MonoBehaviour
{
    public Button resetButton;
    public GameObject[] monoSources;
    public GameObject sourceParent;
    public GameObject audioDropZone;

    //Define UDP port number and IP address to send data to MATLAB
    public int port;
    public string IP = "127.0.0.1";

    //UDP Conncetion elements
    IPEndPoint remoteEndPoint;
    UdpClient client;

    //Initialise string to contain reset status message
    public string statusMessage = "";

    // Start is called before the first frame update
    void Start()
    {
        resetButton.onClick.AddListener(resetAllSources);
        init();
    }
    
    void resetAllSources()
    {
        //Debug.Log("Reset Button Pressed");
        foreach (GameObject soundSource in monoSources)
        {
            soundSource.transform.SetParent(sourceParent.transform);
        }

        Dropzone dropped = audioDropZone.GetComponent<Dropzone>();
        dropped.sourceCounter = 0;

        for(int i = 0; i<10; ++i)
        {
            dropped.sourceActivationFlags[i] = 0;
        }

        sendResetData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Initialise UDP
    public void init()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(this.IP), this.port);
        client = new UdpClient();
    }

    void sendResetData()
    {
        this.statusMessage = "Scene Reset";
        SendString(this.statusMessage);
    }

    void SendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
            //Debug.Log("error here");
        }
    }
}
