﻿using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

class ClientScript : NetworkScript
{
    public Button connectButton;
    public InputField codeInput;
    Utility.ClientColor myColor = Utility.ClientColor.none;

    protected override void Start()
    {
        base.Start();
        connectButton.onClick.AddListener(clientConnect);
    }

    void clientConnect()
    {
        code = codeInput.text;
        var options = new MessageOptions("client_connection", code);
        var message = new WebSocketMessage(options);
        var json = message.toJson();
        webSocket.Send(json.ToString());
    }

    protected override void onOpen()
    {
        connectButton.interactable = true;
    }

    protected override void onMessage(string data)
    {
        Debug.Log(data);
        var message = JSON.Parse(data);
        var options = new MessageOptions(message["options"]);
        switch (options.type)
        {
            case "package_from_host":
                handlePackage(options.packageType, message["package"]);
                break;
            case "color_change":
                myColor = options.color;
                break;
            default:
                break;
        }
    }

    void handlePackage(string type, JSONNode package)
    {
        switch (type)
        {
            case "string":
                var stringPackage = new StringPackage(package);
                Debug.Log(stringPackage.package);
                break;
            default:
                break;
        }
    }

    void sendToHost(IJsonable package, string packageType)
    {
        var options = new MessageOptions("client_to_host", code, packageType: packageType);
        var wbm = new WebSocketMessage(options, package);
        var json = wbm.toJson();
        webSocket.Send(json.ToString());
    }
}