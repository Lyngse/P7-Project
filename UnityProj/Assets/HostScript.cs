using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

class HostScript : NetworkScript
{
    public Button hostButton;
    List<Utility.ClientColor> clientColors = new List<Utility.ClientColor>();

    private void Start()
    {
        hostButton.onClick.AddListener(hostClick);
    }

    void hostClick()
    {
        startNetwork();
    }

    protected override void onOpen()
    {
        var options = new MessageOptions("host_connection");
        var message = new WebSocketMessage(options);
        var json = message.toJson();
        webSocket.Send(json.ToString());
    }

    protected override void onMessage(string data)
    {
        Debug.Log(data);
        var message = JSON.Parse(data);
        var options = new MessageOptions(message["options"]);
        switch (options.type)
        {
            case "code":
                code = options.code;
                hostButton.GetComponentInChildren<Text>().text = code;
                break;
            case "client_joined":
                clientColors.Add(options.color);
                sendToClient(options.color, new StringPackage(options.color.ToString()), "string");
                break;
            default:
                break;
        }
    }

    void sendToClient(Utility.ClientColor clientColor, IJsonable package, string packageType)
    {
        var options = new MessageOptions("host_to_client", code, clientColor, packageType);
        var wbm = new WebSocketMessage(options, package);
        var json = wbm.toJson();
        webSocket.Send(json.ToString());
    }

    void sendToAll(IJsonable message)
    {
        throw new NotImplementedException();
    }
}