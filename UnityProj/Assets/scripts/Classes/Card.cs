using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;
using Assets.scripts;

public class Card : MonoBehaviour, IJsonable
{
    public Texture2D frontImg;
    public Texture2D backImg;
    public string frontImgUrl;
    public string backImgUrl;
    public bool isFaceDown;

    public Card(string front, string back)
    {
        this.frontImgUrl = front;
        this.backImgUrl = back;
        this.isFaceDown = true;
    }

    public void fromJson(JSONNode json)
    {
        throw new NotImplementedException();
    }
        
    public JSONNode toJson()
    {
        JSONNode json = new JSONObject();
        json.Add("FrontImage", new JSONString(frontImgUrl));
        json.Add("BackImage", new JSONString(backImgUrl));
        json.Add("isFaceDown", new JSONBool(isFaceDown));

        return json;
    }

    public void DealToPlayer(Utility.ClientColor color)
    {
        HostScript currentHost = GameObject.Find("NetworkHost").GetComponent<HostScript>();
        currentHost.sendToClient(color, this, "Card");
    }

    public void PlaceCardOnTopOfDeck(Card card)
    {

    }
}

