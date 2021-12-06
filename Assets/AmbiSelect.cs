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

public class AmbiSelect : MonoBehaviour
{
    //Add a button to identify button 
    public Button ambiButton;

    //Define Identifiers for each ambisonic scene
    public int ambiSourceIndex;

    //Define UDP port number and IP address to send data to MATLAB
    public int port;
    public string IP = "127.0.0.1";

    //UDP Conncetion elements
    IPEndPoint remoteEndPoint;
    UdpClient client;

    //Intialise string to send data to MATLAB
    public string statusMessage = "";

    // Start is called before the first frame update
    void Start()
    {
        ambiButton.onClick.AddListener(sendAmbiData);
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void sendAmbiData()
    {
        this.statusMessage = "" + this.ambiSourceIndex;
        SendString(this.statusMessage);
    }

    //Main
    private static void Main()
    {
        AmbiSelect sendOBJ = new AmbiSelect();
        sendOBJ.init();
    }

    //Intiialise UDP 
    public void init()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(this.IP), this.port);
        client = new UdpClient();
    }

    //Send string over UDP
    private void SendString(string message)
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
