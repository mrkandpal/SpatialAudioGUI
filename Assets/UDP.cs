using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using UnityEngine.UI;

public class UDP : MonoBehaviour
{
    public static string sendText;
    private static int localPort;
    private string sendTextOld;

    public Button UDPButton;
    public int counter = 0;

    // prefs
    private string IP;  // define in init
    public int port;  // define in init

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // call it from shell (as program)
    private static void Main()
    {
        UDP sendObj = new UDP();
        sendObj.init();

        // testing via console
        // sendObj.inputFromConsole();

        // as server sending endless
        sendObj.sendEndless(" endless infos \n");
    }

    // Start is called before the first frame update
    void Start()
    {
        sendText = "";
        sendTextOld = "";
        init();

        UDPButton.onClick.AddListener(SendUDP);
    }

    // Update is called once per frame
    void Update()
    {


        if(sendText != sendTextOld)
        {
            sendString(sendText);
            Debug.Log(sendText);
            sendTextOld = sendText;
        }
    }

    void SendUDP()
    {
        sendText = "Sending a Test String: " + counter;
        counter++;
    }

    //init
    public void init()
    {
        print("UDP.init()");
        //define IP address and port number
        IP = "127.0.0.1";
        port = 8085;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        // status
        //  print("Sending to " + IP + " : " + port);
        //  print("Testing: nc -lu " + IP + " : " + port);
    }

    //InputFromConsole
    private void inputFromConsole()
    {
        try
        {
            string text;
            do
            {
                text = Console.ReadLine();

                if (text != "")
                {
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    client.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }

    //sendData
    private void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            Debug.Log("Send " + message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch(Exception err)
        {
            print(err.ToString());
        }
        
    }

    //endless test
    private void sendEndless(string testStr)
    {
        do
        {
            sendString(testStr);
        } while (true);
    }
}
