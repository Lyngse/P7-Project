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

    protected virtual void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(connectToWebsocket());
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
    }

    private void OnApplicationQuit()
    {
        webSocket.Close();
    }

    protected abstract void onClose();

    protected abstract void onOpen();

    protected abstract void onMessage(string data);
}
