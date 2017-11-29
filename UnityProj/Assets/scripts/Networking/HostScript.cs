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

    public Canvas overlayCanvas;
    ClientStateHandler clientStateHandler;

    private void Start()
    {
        clientStateHandler = new ClientStateHandler();
        base.Start();
    }

    protected override void onOpen()
    {
        overlayCanvas.gameObject.SetActive(false);
        var options = new MessageOptions("host_connection");
        var message = new WebSocketMessage(options);
        var json = message.toJson();
        webSocket.SendString(json.ToString());
    }

    protected override void onClose()
    {
        Debug.Log("Connection Lost!");
        overlayCanvas.gameObject.SetActive(true);
        GameObject.Find("ReconnectButton").GetComponent<Button>().enabled = true;
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
                Transform menuField = GameObject.Find("CodeField").transform.GetChild(0);
                menuField.GetComponent<Text>().text = code;
                break;
            case "client_joined":
                var objects1 = clientStateHandler.colorConnected(options.color);
                foreach (var item in objects1)
                {
                    localSendToClient(options.color, item.Second, item.First);
                }
                break;
            case "client_disconnected":
                Debug.Log("Client Disconnected: " + options.color.ToString());
                clientStateHandler.colorDisconnected(options.color);
                break;
            case "color_change":
                ColorChangePackage ccPackage = new ColorChangePackage(message["package"]);
                clientStateHandler.colorDisconnected(ccPackage.fromColor);
                var objects2 = clientStateHandler.colorConnected(ccPackage.toColor);
                foreach (var item in objects2)
                {
                    localSendToClient(ccPackage.toColor, item.Second, item.First);
                }
                break;
            case "package_from_client":
                HandleClientPackage(options.packageType, message["package"], options.color);
                break;
            default:
                break;
        }
    }

    private void HandleClientPackage(string type, JSONNode package, Utility.ClientColor color)
    {
        switch (type)
        {
            case "card":
                var cardPrefab = Resources.Load<Transform>("Prefabs/Card");
                Transform newCard = Instantiate(cardPrefab);
                newCard.GetComponent<Card>().Instantiate(package);
                clientStateHandler.removeObjectFromColor(color, newCard.GetComponent<Card>(), type);
                break;
            case "figurine":

                break;
            default:
                break;
        }
    }

    public void sendToClient(Utility.ClientColor clientColor, IJsonable package, string packageType)
    {
        localSendToClient(clientColor, package, packageType);
        clientStateHandler.addObjectToColor(clientColor, package, packageType);
    }

    private void localSendToClient(Utility.ClientColor clientColor, IJsonable package, string packageType)
    {
        var options = new MessageOptions("host_to_client", code, clientColor, packageType);
        var wbm = new WebSocketMessage(options, package);
        var json = wbm.toJson();
        webSocket.SendString(json.ToString());
    }

    public void reconnect()
    {
        StartCoroutine(connectToWebsocket());
    }

    public void sendToAll(IJsonable message)
    {
        throw new NotImplementedException();
    }

    public List<Utility.ClientColor> getConnectedColors()
    {
        return clientStateHandler.getConnectedColors();
    }

    class ClientStateHandler
    {
        Tuple<bool, List<Tuple<string, IJsonable>>>[] clients = new Tuple<bool, List<Tuple<string, IJsonable>>>[Utility.colors.Length];

        public ClientStateHandler()
        {
            for (int i = 0; i < clients.Length; i++)
            {
                clients[i] = new Tuple<bool, List<Tuple<string, IJsonable>>>(false, new List<Tuple<string, IJsonable>>());
            }
        }

        public List<Tuple<string, IJsonable>> colorConnected(Utility.ClientColor color)
        {
            clients[(int)color].First = true;
            return clients[(int)color].Second;
        }

        public void colorDisconnected(Utility.ClientColor color)
        {
            clients[(int)color].First = false;
        }

        public void addObjectToColor(Utility.ClientColor color, IJsonable obj, string type)
        {
            clients[(int)color].Second.Add(new Tuple<string, IJsonable>(type, obj));
        }

        public void removeObjectFromColor(Utility.ClientColor color, IJsonable obj, string type)
        {
            var objects = clients[(int)color].Second;

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].Second.toJson().ToString().Equals(obj.toJson().ToString()))
                {
                    objects.RemoveAt(i);
                    break;
                }
            }
            
        }

        public List<Utility.ClientColor> getConnectedColors()
        {
            List<Utility.ClientColor> result = new List<Utility.ClientColor>();
            for (int i = 0; i < clients.Length; i++)
            {
                if (clients[i].First)
                    result.Add((Utility.ClientColor)i);
            }
            return result;
        }
    }

}