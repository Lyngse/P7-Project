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
    protected WebSocket webSocket = new WebSocket(new Uri("ws://p7-webserver.herokuapp.com"));
    bool open = false;
    protected string code = "";
    public Queue<Tuple<Utility.websocketEvent, string>> bufferQueue = new Queue<Tuple<Utility.websocketEvent, string>>();

    protected virtual void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(connectToWebsocket());
        //webSocket.OnMessage += socketOnMessage;
        //webSocket.OnClose += socketOnClose;
        Timer timer = new Timer(500);
        timer.Elapsed += new ElapsedEventHandler(Ping);
        timer.Enabled = true;
    }

    protected IEnumerator connectToWebsocket()
    {
        webSocket = new WebSocket(new Uri("ws://p7-webserver.herokuapp.com"));
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

    private void Ping(object source, ElapsedEventArgs e)
    {
        if(open)
            webSocket.SendString(new JSONString("ping").ToString());
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
