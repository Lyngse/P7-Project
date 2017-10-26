using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using SimpleJSON;
using UnityEngine;

abstract class NetworkScript : MonoBehaviour
{
    protected WebSocket webSocket = new WebSocket("ws://p7-webserver.herokuapp.com");
    protected string code = "";
    public Queue<Tuple<Utility.websocketEvent, string>> bufferQueue = new Queue<Tuple<Utility.websocketEvent, string>>();
    bool started = false;

    private void Update()
    {
        if (bufferQueue.Count > 0)
        {
            var websocketEvent = bufferQueue.Dequeue();
            switch (websocketEvent.First)
            {
                case Utility.websocketEvent.Open:
                    onOpen();
                    break;
                case Utility.websocketEvent.Message:
                    onMessage(websocketEvent.Second);
                    break;
                default:
                    break;
            }
        }
    }

    protected abstract void onOpen();

    protected abstract void onMessage(string data);

    public void startNetwork()
    {
        Application.runInBackground = true;
        webSocket.ConnectAsync();
        webSocket.OnOpen += socketOnOpen;
        webSocket.OnMessage += socketOnMessage;
        
    }

    void socketOnOpen(object sender, EventArgs e)
    {
        bufferQueue.Enqueue(Tuple.New(Utility.websocketEvent.Open, ""));
    }
    void socketOnMessage(object sender, MessageEventArgs e)
    {
        bufferQueue.Enqueue(Tuple.New(Utility.websocketEvent.Message, e.Data));
    }

    protected JSONObject createMessage(string type, IJsonable payload)
    {
        var json = new JSONObject();
        json.Add("type", new JSONString(type));
        var payloadJson = payload.toJson();
        json.Add("payload", payloadJson);
        return json;
    }

    protected JSONObject createMessage(string type, string payload)
    {
        var json = new JSONObject();
        json.Add("type", new JSONString(type));
        json.Add("payload", new JSONString(payload));
        return json;
    }
}
