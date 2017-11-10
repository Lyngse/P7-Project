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

    public InputField codeField;
    public List<Utility.ClientColor> clientColors = new List<Utility.ClientColor>();

    protected override void onOpen()
    {
        var options = new MessageOptions("host_connection");
        var message = new WebSocketMessage(options);
        var json = message.toJson();
        webSocket.Send(json.ToString());
    }

    protected override void onClose()
    {
        Debug.Log("Connection Lost!");
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
                codeField.text = code;
                break;
            case "client_joined":
                clientColors.Add(options.color);
                sendToClient(options.color, new StringPackage(options.color.ToString()), "string");
                break;
            case "client_disconnected":
                Debug.Log("Client Disconnected: " + options.color.ToString());
                clientColors.Remove(options.color);
                break;
            case "package_from_client":
                HandleClientPackage(options.packageType, message["package"]);
                break;
            default:
                break;
        }
    }

    private void HandleClientPackage(string type, JSONNode package)
    {
        switch (type)
        {
            case "card":
                var cardPrefab = Resources.Load<Transform>("Prefabs/Card");
                Transform newCard = Instantiate(cardPrefab);
                newCard.GetComponent<Card>().Instantiate(package);
                newCard.position = new Vector3((transform.position.x + 7), 5, transform.position.z);
                newCard.gameObject.SetActive(true);
                break;
            case "figurine":

                break;
            default:
                break;
        }
    }

    public void sendToClient(Utility.ClientColor clientColor, IJsonable package, string packageType)
    {
        var options = new MessageOptions("host_to_client", code, clientColor, packageType);
        var wbm = new WebSocketMessage(options, package);
        var json = wbm.toJson();
        webSocket.Send(json.ToString());
    }

    public void sendToAll(IJsonable message)
    {
        throw new NotImplementedException();
    }
}