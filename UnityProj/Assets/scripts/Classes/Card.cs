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
    string frontImgUrl;
    string backImgUrl;
    int index;
    int deckId;
    bool isFaceDown;
    WWWController wwwController;

    public void Instantiate(int deckId, int index, string frontImgUrl, string backImgUrl, bool isFaceDown = true)
    {
        this.frontImgUrl = frontImgUrl;
        this.backImgUrl = backImgUrl;
        this.isFaceDown = isFaceDown;
        this.index = index;
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
        if(wwwController == null)
            wwwController = GameObject.Find("SceneScripts").GetComponent<WWWController>();
        Texture2D sourceFrontTex;
        Texture2D sourceBackTex;
        if (wwwController.deckDict.ContainsKey(deckId))
        {
            sourceFrontTex = wwwController.deckDict[deckId].First;
            sourceBackTex = wwwController.deckDict[deckId].Second;

            var backTex = sourceBackTex;

            var cardHeight = sourceFrontTex.height / 7f;
            var cardWidth = sourceFrontTex.width / 10f;
            var frontTex = CropImageToCard(sourceFrontTex, ((index / 7) * cardWidth), ((index % 7) * cardHeight), cardWidth, cardHeight);

            transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = frontTex;
            transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = backTex;
        }
        else
        {
            StartCoroutine(wwwController.getDeck(deckId, frontImgUrl, backImgUrl, init));
        }
        
    }

    public void fromJson(JSONNode json)
    {
        frontImgUrl = json["FrontImage"].Value;
        backImgUrl = json["BackImage"].Value;
        isFaceDown = json["isFaceDown"].AsBool;
        index = json["index"].AsInt;
        deckId = json["deckId"].AsInt;
    }

    public JSONNode toJson()
    {
        JSONNode json = new JSONObject();
        json.Add("FrontImage", new JSONString(frontImgUrl));
        json.Add("BackImage", new JSONString(backImgUrl));
        json.Add("isFaceDown", new JSONBool(isFaceDown));
        json.Add("index", new JSONNumber(index));
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

