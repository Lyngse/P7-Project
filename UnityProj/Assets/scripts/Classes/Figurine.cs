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
        //InstantiateFigurine("http://www.google.fr/url?source=imglanding&ct=img&q=http://mywastedlife.com/CAH/img/back-white.png&sa=X&ved=0CAkQ8wdqFQoTCIOlwO7IhcYCFQFYFAodYnoAUg&usg=AFQjCNGdlrUGLinNrm18KedLAfCNPW3x6w");
    }

    public void Instantiate(JSONNode json)
    {
        fromJson(json);
        InstantiateFigurine();
    }

    public void Instantiate(string figurineTexUrl, string meshURL, JSONNode json)
    {
        fromJson(json);
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
        //
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
}
