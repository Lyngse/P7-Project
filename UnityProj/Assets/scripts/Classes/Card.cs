using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;
using Assets.scripts;
using System.Collections;

public class Card : MonoBehaviour, IJsonable
{
    public string frontImgUrl;
    public string backImgUrl;
    public int id;
    public bool isFaceDown;
    WWWController wwwController;

    public void Instantiate(int id, string frontImgUrl, string backImgUrl, bool isFaceDown = true)
    {
        this.frontImgUrl = frontImgUrl;
        this.backImgUrl = backImgUrl;
        this.isFaceDown = isFaceDown;
        this.id = id;
        init();
    }

    public void Instantiate(JSONNode json)
    {
        fromJson(json);
        init();
    }

    void init()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = Resources.Load<Texture2D>("loading");
        transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = Resources.Load<Texture2D>("loading");

        wwwController = GameObject.Find("SceneScripts").GetComponent<WWWController>();
        wwwController.GetCard(id, frontImgUrl, backImgUrl, (textures =>
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = textures.First;
            transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = textures.Second;
        }));
        if (!isFaceDown)
        {
            transform.Rotate(transform.rotation.x + 180.0f, 0, 0);
        }
    }

    public void fromJson(JSONNode json)
    {
        frontImgUrl = json["FrontImage"].Value;
        backImgUrl = json["BackImage"].Value;
        isFaceDown = json["isFaceDown"].AsBool;
        id = json["index"].AsInt;
    }

    public JSONNode toJson()
    {
        JSONNode json = new JSONObject();
        json.Add("FrontImage", new JSONString(frontImgUrl));
        json.Add("BackImage", new JSONString(backImgUrl));
        json.Add("isFaceDown", new JSONBool(isFaceDown));
        json.Add("index", new JSONNumber(id));

        return json;
    }

    public void DealToPlayer(Utility.ClientColor color)
    {
        HostScript currentHost = GameObject.Find("NetworkHost").GetComponent<HostScript>();
        currentHost.sendToClient(color, this, "card");
    }

    public void PlaceCardOnTopOfDeck(Card card)
    {

    }
}

