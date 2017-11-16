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
    int deckId;
    public bool isFaceDown;
    WWWController wwwController;

    public void Instantiate(int deckId, int id, string frontImgUrl, string backImgUrl, bool isFaceDown = true)
    {
        this.frontImgUrl = frontImgUrl;
        this.backImgUrl = backImgUrl;
        this.isFaceDown = isFaceDown;
        this.id = id;
        this.deckId = deckId;
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
        wwwController.GetCard(id, deckId, frontImgUrl, backImgUrl, (textures =>
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = textures.First;
            transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = textures.Second;
        }));
        if (!isFaceDown)
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }
    }

    public void fromJson(JSONNode json)
    {
        frontImgUrl = json["FrontImage"].Value;
        backImgUrl = json["BackImage"].Value;
        isFaceDown = json["isFaceDown"].AsBool;
        id = json["index"].AsInt;
        deckId = json["deckId"].AsInt;
    }

    public JSONNode toJson()
    {
        JSONNode json = new JSONObject();
        json.Add("FrontImage", new JSONString(frontImgUrl));
        json.Add("BackImage", new JSONString(backImgUrl));
        json.Add("isFaceDown", new JSONBool(isFaceDown));
        json.Add("index", new JSONNumber(id));
        json.Add("deckId", new JSONNumber(deckId));

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

    public Texture2D CropImageToCard(Texture2D sourceTex, float sourceX, float sourceY, float sourceWidth, float sourceHeight)
    {
        int x = Mathf.FloorToInt(sourceX);
        int y = Mathf.FloorToInt(sourceY);
        int width = Mathf.FloorToInt(sourceWidth);
        int height = Mathf.FloorToInt(sourceHeight);

        Color[] pix = sourceTex.GetPixels(x, y, width, height);
        Texture2D destTex = new Texture2D(width, height);
        destTex.SetPixels(pix);
        destTex.Apply();
        return destTex;
    }
}

