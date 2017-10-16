using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class ClientScript : MonoBehaviour {

    WebSocket webSocket = new WebSocket("ws://192.168.0.100:5000");
    WebSocket webSocket2 = new WebSocket("ws://192.168.0.100:5000");
    List<string> chatClients = new List<string>();
    private bool hasSent = false;

    // Use this for initialization
    void Start () {
        webSocket.Connect();

        webSocket.OnMessage += (sender, e) =>
        {
            Debug.Log("Node says: " + e.Data);
            chatClients.Add(e.Data);
        };

      

    }
	
	// Update is called once per frame
	void Update () {
	    if (webSocket.ReadyState == WebSocketState.Open && hasSent == false && chatClients.Count != 0)
	    {
	        messageObject testMessage = new messageObject("forward", chatClients[0]);
	        var jsonMessage = JsonUtility.ToJson(testMessage);
	        webSocket.Send(jsonMessage);
	        hasSent = true;
	    }

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

        public messageObject(string thisType, string thisReciever)
        {
            type = thisType;
            reciever = thisReciever;
        }
    }







}
