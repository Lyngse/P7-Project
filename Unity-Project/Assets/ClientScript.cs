﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class ClientScript : MonoBehaviour {

    WebSocket webSocket = new WebSocket("ws://localhost:5000");
    List<string> chatClients = new List<string>();
    private bool hasSent = false;

    // Use this for initialization
    void Start ()
    {
        StartServer();
        webSocket.ConnectAsync();

        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Node says: " + e.Data);
            chatClients.Add(e.Data);
        };

      

    }
	
	// Update is called once per frame
	void Update () {
	    if (webSocket.ReadyState == WebSocketState.Open && hasSent == false && chatClients.Count > 1)
	    {
	        messageObject testMessage = new messageObject("forward", chatClients[1], "Søren sucks");
	        var jsonMessage = JsonUtility.ToJson(testMessage);
	        //webSocket.Send(jsonMessage);
	        hasSent = true;
	    }

    }

    private void StartServer()
    {
        var process = new System.Diagnostics.Process();
        process.StartInfo.FileName = @".\..\webSocketServer\startserver.bat";
        process.StartInfo.WorkingDirectory = @".\..\webSocketServer";
        process.Start();

    }


    public void recieveMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);

        
    }

    public class messageObject
    {
        // Possibly make the type an enum
        public string type;
        public string reciever;
        public string message;

        public messageObject(string thisType, string thisReciever, string thisMessage = "")
        {
            type = thisType;
            reciever = thisReciever;
            message = thisMessage;
        }
    }







}