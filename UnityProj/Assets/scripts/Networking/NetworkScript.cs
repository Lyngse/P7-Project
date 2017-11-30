using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using SimpleJSON;
using UnityEngine;
using System.Timers;
using System.Collections;

abstract class NetworkScript : MonoBehaviour
{
    protected WebSocket webSocket = new WebSocket(new Uri("wss://p7-webserver.herokuapp.com"));
    bool open = false;
    float pingTimer;
    protected string code = "";
    public Queue<Tuple<Utility.websocketEvent, string>> bufferQueue = new Queue<Tuple<Utility.websocketEvent, string>>();

    protected virtual void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(connectToWebsocket());
        //webSocket.OnMessage += socketOnMessage;
        //webSocket.OnClose += socketOnClose;
        var pingTimer = Time.realtimeSinceStartup;
    }

    protected IEnumerator connectToWebsocket()
    {
        webSocket = new WebSocket(new Uri("wss://p7-webserver.herokuapp.com"));
        yield return StartCoroutine(webSocket.Connect());
        onOpen();
        open = true;
        
    }
    
    private void Update()
    {
        string recievedString = webSocket.RecvString();
        if(recievedString != null)
        {
            onMessage(recievedString);
        }
        if(webSocket.error != null && open)
        {
            Debug.Log(webSocket.error);
            webSocket.Close();
            open = false;
            onClose();
        }
        if (open)
        {
            float deltaTime = Time.realtimeSinceStartup - pingTimer;
            if(deltaTime > 500)
            {
                Debug.Log("ping");
                webSocket.SendString(new JSONString("ping").ToString());
                pingTimer = Time.realtimeSinceStartup;
            }
        }
        //if (bufferQueue.Count > 0)
        //{
        //    var websocketEvent = bufferQueue.Dequeue();
        //    switch (websocketEvent.First)
        //    {
        //        case Utility.websocketEvent.Open:
        //            onOpen();
        //            break;
        //        case Utility.websocketEvent.Close:
        //            onClose();
        //            break;
        //        case Utility.websocketEvent.Message:
        //            onMessage(websocketEvent.Second);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

    private void OnApplicationQuit()
    {
        webSocket.Close();
    }

    protected abstract void onClose();

    protected abstract void onOpen();

    protected abstract void onMessage(string data);

    void socketOnOpen(object sender, EventArgs e)
    {
        bufferQueue.Enqueue(Tuple.New(Utility.websocketEvent.Open, ""));
    }

    void socketOnClose(object sender, CloseEventArgs e)
    {
        bufferQueue.Enqueue(Tuple.New(Utility.websocketEvent.Close, ""));
    }

    void socketOnMessage(object sender, MessageEventArgs e)
    {
        bufferQueue.Enqueue(Tuple.New(Utility.websocketEvent.Message, e.Data));
    }
}
