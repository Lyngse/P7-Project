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
            Texture2D figurineTexture = new Texture2D(4, 4);
            var colors = new Color[figurineTexture.GetPixels().Length];
            switch (figurineUrl)
            {
                
                case "":
                    setTextureToColor(Utility.ClientColor.none, figurineTexture);
                    break;
                case "red":
                    setTextureToColor(Utility.ClientColor.red, figurineTexture);
                    break;
                case "green":
                    setTextureToColor(Utility.ClientColor.green, figurineTexture);
                    break;
                case "blue":
                    setTextureToColor(Utility.ClientColor.blue, figurineTexture);
                    break;
                case "white":
                    setTextureToColor(Utility.ClientColor.white, figurineTexture);
                    break;
                case "black":
                    setTextureToColor(Utility.ClientColor.black, figurineTexture);
                    break;
                case "yellow":
                    setTextureToColor(Utility.ClientColor.yellow, figurineTexture);
                    break;
                case "orange":
                    setTextureToColor(Utility.ClientColor.orange, figurineTexture);
                    break;
                case "purple":
                    setTextureToColor(Utility.ClientColor.purple, figurineTexture);
                    break;
                default:
                    WWW figurineWWW = new WWW(figurineUrl);
                    yield return figurineWWW;
                    figurineTexture = figurineWWW.texture;
                    break;
            }
            WWW meshWWW = new WWW(meshUrl);
            
            yield return meshWWW;
            ObjImporter importer = new ObjImporter();
            Mesh mesh = importer.ImportFile(meshWWW.text);
            mesh.RecalculateNormals();
            result = new Tuple<Texture2D, Mesh>(figurineTexture, mesh);
            if (!figurineDict.ContainsKey(identifier))
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
            if(!deckDict.ContainsKey(identifier))
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

    private void setTextureToColor(Utility.ClientColor color, Texture2D texture)
    {
        var pixels = new Color[texture.GetPixels().Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Utility.colors[(int)color];
        }
        texture.SetPixels(pixels);
        texture.Apply();

    }

    private Texture2D CropImageToCard(Texture2D sourceTex, int cardID)
    {
        var cardHeight = sourceTex.height / 7f;
        var cardWidth = sourceTex.width / 10f;

        float sourceX = ((cardID % 10) * cardWidth);
        float sourceY = (sourceTex.height - (cardHeight + (cardHeight * (Mathf.FloorToInt(cardID / 10)))));

        //float sourceX = ((cardID / 7) * cardWidth);
        //float sourceY = ((cardID % 7) * cardHeight);
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
