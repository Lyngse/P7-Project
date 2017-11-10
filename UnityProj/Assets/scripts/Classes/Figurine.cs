using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;

public class Figurine : MonoBehaviour, IJsonable
{
    private int id;
    private string figurineTextureUrl;
    private string meshUrl;
    WWWController wwwcontroller;

    private void Start()
    {
        Instantiate("http://www.berserk-games.com/images/TTS-Coin-Template.png", "http://pastebin.com/raw.php?i=00YWZ28Y");
    }

    public void Instantiate(JSONNode json)
    {
        fromJson(json);
        InstantiateFigurine();
    }

    public void Instantiate(string figurineTexUrl, string meshURL)
    {
        figurineTextureUrl = figurineTexUrl;
        meshUrl = meshURL;
        wwwcontroller = GameObject.Find("SceneScripts").GetComponent<WWWController>();
        id = wwwcontroller.CreateFigurine(figurineTextureUrl, meshUrl, x => WrapFigurine(x.First, x.Second));
    }

    public void InstantiateFigurine()
    {
        wwwcontroller = GameObject.Find("SceneScripts").GetComponent<WWWController>();
        id = wwwcontroller.CreateFigurine(figurineTextureUrl, meshUrl, x => WrapFigurine(x.First, x.Second));
    }

    private void WrapFigurine(Texture2D texture, Mesh mesh)
    {
        transform.GetComponent<MeshRenderer>().material.mainTexture = texture;
        transform.GetComponent<MeshFilter>().mesh = mesh;        
        transform.gameObject.tag = "Figurine";
    }

    public void fromJson(JSONNode json)
    {
        this.id = json["index"].AsInt;
        this.figurineTextureUrl = json["figurineImage"].Value;
        this.meshUrl = json["mesh"].Value;
    }

    public JSONNode toJson()
    {
        JSONNode json = new JSONObject();
        json.Add("index", new JSONNumber(id));
        json.Add("figurineImage", new JSONString(figurineTextureUrl));
        json.Add("mesh", new JSONString(meshUrl));

        return json;
    }

    public void DealToPlayer(Utility.ClientColor color)
    {
        HostScript currentHost = GameObject.Find("NetworkHost").GetComponent<HostScript>();
        currentHost.sendToClient(color, this, "figurine");
    }
}
