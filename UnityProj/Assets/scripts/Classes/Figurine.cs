using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;

public class Figurine : MonoBehaviour, IJsonable
{
    private int id;
    public string figurineTextureUrl;
    public string meshUrl;
    WWWController wwwcontroller;

    public void Instantiate(JSONNode json)
    {
        fromJson(json);
        InstantiateFigurine();
    }

    public Figurine Instantiate(string name, string figurineTexUrl, string meshURL)
    {
        figurineTextureUrl = figurineTexUrl;
        meshUrl = meshURL;
        this.name = name;
        InstantiateFigurine();
        return this;
    }

    public void InstantiateFigurine()
    {
        if (GetComponent<Transform>() == null)
            return;
        wwwcontroller = GameObject.Find("SceneScripts").GetComponent<WWWController>();
        StartCoroutine(wwwcontroller.GetFigurine(figurineTextureUrl, meshUrl, x => WrapFigurine(x.First, x.Second)));
    }

    private void WrapFigurine(Texture2D texture, Mesh mesh)
    {
        transform.GetComponent<MeshRenderer>().material.mainTexture = texture;
        transform.GetComponent<MeshFilter>().mesh = mesh;
        transform.GetComponent<MeshCollider>().sharedMesh = mesh;  
        var scale = Vector3.one;
        scale.x = -scale.x;
        transform.localScale = scale;
        transform.gameObject.tag = "Figurine";
        transform.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void fromJson(JSONNode json)
    {
        this.figurineTextureUrl = json["figurineImage"].Value;
        this.meshUrl = json["mesh"].Value;
        if (json["name"] != null)
        {
            this.name = json["name"].Value;
        }
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
