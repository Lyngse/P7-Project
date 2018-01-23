﻿using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

class ClientScript : NetworkScript
{
    public Canvas connectCanvas;
    public HandController handController;
    Button connectButton;
    InputField codeInput;
    Text colorText;
    Utility.ClientColor myColor = Utility.ClientColor.none;
    public GameObject colorInfo;

    protected override void Start()
    {
        base.Start();
        codeInput = connectCanvas.GetComponentInChildren<InputField>();
        connectButton = connectCanvas.GetComponentInChildren<Button>();
        connectButton.onClick.AddListener(clientConnect);  
        colorText = colorInfo.GetComponentsInChildren<Text>().First(text => text.name == "colorText");
        //JSONNode json = new JSONObject();
        //json.Add("FrontImage", new JSONString("https://i.imgur.com/iSAo3YC.jpg"));
        //json.Add("BackImage", new JSONString("https://i.imgur.com/PwhF8u0.jpg"));
        //json.Add("isFaceDown", new JSONBool(false));
        //json.Add("index", new JSONNumber(0));
        //for (int i = 0; i < 12; i++)
        //{
        //    json.Remove("index");
        //    json.Add("index", new JSONNumber(i));
        //    handlePackage("card", json);
        //}

    }

    void clientConnect()
    {
        code = codeInput.text;
        var options = new MessageOptions("client_connection", code);
        var message = new WebSocketMessage(options);
        var json = message.toJson();
        webSocket.SendString(json.ToString());
    }

    protected override void onOpen()
    {
        connectButton.interactable = true;
    }

    protected override void onClose()
    {
        
        resetValues();
        connectButton.interactable = false;
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void exit()
    {
        onClose();
        webSocket.Close();

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
                resetValues();
                myColor = options.color;
                connectCanvas.gameObject.SetActive(false);
                colorInfo.SetActive(true);
                colorText.text = myColor.ToString();
                colorText.color = Utility.colors[(int)myColor];
                break;
            case "host_disconnected":
                Debug.Log("host disconnected");
                resetValues();
                break;
            case "error":
                string package = message["package"];
                if(package.StartsWith("no game with code"))
                {
                    Debug.Log("No game found");
                }
                else if(package == "game is full")
                {
                    Debug.Log("Game is full");
                }
                break;
            default:
                break;
        }
    }

    void resetValues()
    {
        myColor = Utility.ClientColor.none;
        connectCanvas.gameObject.SetActive(true);
        colorInfo.SetActive(false);
        handController.ClearHand();
        colorText.text = "";
        colorText.color = Utility.colors[0];
        codeInput.text = "";
    }

    void handlePackage(string type, JSONNode package)
    {
        switch (type)
        {
            case "string":
                var stringPackage = new StringPackage(package);
                Debug.Log(stringPackage.package);
                break;
            case "card":
                handController.addCard(package);
                break;
            case "figurine":
                handController.addFigurine(package);
                break;
            default:
                break;
        }
    }

    public void sendToHost(IJsonable package, string packageType)
    {
        var options = new MessageOptions("client_to_host", code, myColor, packageType);
        var wbm = new WebSocketMessage(options, package);
        var json = wbm.toJson();
        webSocket.SendString(json.ToString());
    }

    public void changeColor(int newColorInt)
    {
        var newColor = (Utility.ClientColor)newColorInt;
        if(newColor == myColor) return;
        var options = new MessageOptions("color_change", code);
        var package = new ColorChangePackage(myColor, newColor);
        var wbm = new WebSocketMessage(options, package);
        var json = wbm.toJson();
        webSocket.SendString(json.ToString());
    }
}