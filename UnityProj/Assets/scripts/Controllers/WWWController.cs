using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.scripts;

public class WWWController: MonoBehaviour
{
    Dictionary<string, Tuple<Texture2D, Texture2D>> deckDict = new Dictionary<string, Tuple<Texture2D, Texture2D>>();
    Dictionary<string, Tuple<Texture2D, Mesh>> figurineDict = new Dictionary<string, Tuple<Texture2D, Mesh>>();

    public IEnumerator GetFigurine(string figurineUrl, string meshUrl, Action<Tuple<Texture2D, Mesh>> callback)
    {
        string identifier = figurineUrl + meshUrl;
        Tuple<Texture2D, Mesh> result;
        if (figurineDict.ContainsKey(identifier))
        {
            result = figurineDict[identifier];            
        } else
        {
            WWW figurineWWW = new WWW(figurineUrl);
            WWW meshWWW = new WWW(meshUrl);
            yield return figurineWWW;
            yield return meshWWW;
            ObjImporter importer = new ObjImporter();
            Mesh mesh = importer.ImportFile(meshWWW.text);
            mesh.RecalculateNormals();
            result = new Tuple<Texture2D, Mesh>(figurineWWW.texture, mesh);
            figurineDict.Add(identifier, result);
        }
        callback(result);
    }

    public IEnumerator getDeck(string frontUrl, string backUrl, Action<Tuple<Texture2D, Texture2D>> callback)
    {
        string identifier = frontUrl + backUrl;
        Tuple<Texture2D, Texture2D> result;
        if (deckDict.ContainsKey(identifier))
        {
            result = deckDict[identifier];
        }
        else
        {
            WWW frontWWW = new WWW(frontUrl);
            WWW backWWW = new WWW(backUrl);
            yield return frontWWW;
            yield return backWWW;
            result = new Tuple<Texture2D, Texture2D>(frontWWW.texture, backWWW.texture);
            deckDict.Add(identifier, result);
        }        
        callback(result);
    }

    public void GetCard(int cardID, string frontUrl, string backUrl, Action<Tuple<Texture2D, Texture2D>> callback)
    {
        string identifier = frontUrl + backUrl;
        if (deckDict.ContainsKey(identifier))
        {
            Tuple<Texture2D, Texture2D> deckTextures = deckDict[identifier];            
            var frontTex = CropImageToCard(deckTextures.First, cardID);
            var backTex = deckTextures.Second;

            callback(new Tuple<Texture2D, Texture2D>(frontTex, backTex));
        } else
        {
            StartCoroutine(getDeck(frontUrl, backUrl, (deckTextures => {
                var frontTex = CropImageToCard(deckTextures.First, cardID);
                var backTex = deckTextures.Second;
                callback(new Tuple<Texture2D, Texture2D>(frontTex, backTex));
            })));
        }
    }

    private Texture2D CropImageToCard(Texture2D sourceTex, int cardID)
    {
        var cardHeight = sourceTex.height / 7f;
        var cardWidth = sourceTex.width / 10f;
        float sourceX = ((cardID / 7) * cardWidth);
        float sourceY = ((cardID % 7) * cardHeight);
        int x = Mathf.FloorToInt(sourceX);
        int y = Mathf.FloorToInt(sourceY);
        int width = Mathf.FloorToInt(cardWidth);
        int height = Mathf.FloorToInt(cardHeight);

        Color[] pix = sourceTex.GetPixels(x, y, width, height);
        Texture2D destTex = new Texture2D(width, height);
        destTex.SetPixels(pix);
        destTex.Apply();
        return destTex;
    }

}
