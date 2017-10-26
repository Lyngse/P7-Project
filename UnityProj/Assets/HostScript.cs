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
        var message = createMessage("host_connection", "");
        string json = message.ToString();
        webSocket.Send(json);
    }

    protected override void onMessage(string data)
    {
        Debug.Log(data);
        var message = JSON.Parse(data);
        var type = message["type"].Value;
        switch (type)
        {
            case "code":
                code = message["payload"].Value;
                hostButton.GetComponentInChildren<Text>().text = code;
                break;
            case "player_joined":
                int colorNumber = message["payload"].AsInt;
                clientColors.Add((Utility.ClientColor)colorNumber);
                sendToClient(clientColors[0], new StringMessage("hello from host"));
                break;
            default:
                break;
        }
    }

    void sendToClient(Utility.ClientColor clientColor, IJsonable message)
    {
        var payload = new toClientMessage();
        payload.code = code;
        payload.clientColor = clientColor;
        payload.message = message;
        var m = createMessage("host_to_client", payload);
        string json = m.ToString();
        webSocket.Send(json);
    }

    void sendToAll(IJsonable message)
    {
        throw new NotImplementedException();
    }
}