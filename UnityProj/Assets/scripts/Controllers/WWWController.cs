using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WWWController: MonoBehaviour
{
    Dictionary<int, Tuple<Texture2D, Texture2D>> deckDict = new Dictionary<int, Tuple<Texture2D, Texture2D>>();
    int nextID = 0;

    public int CreateDeck(string frontUrl, string backUrl, Action<Tuple<Texture2D, Texture2D>> callback)
    {
        int id = nextID;
        nextID++;
        StartCoroutine(getDeck(id, frontUrl, backUrl, callback));
        return id;
    }

    public IEnumerator getDeck(int id, string frontUrl, string backUrl, Action<Tuple<Texture2D, Texture2D>> callback)
    {
        WWW frontWWW = new WWW(frontUrl);
        WWW backWWW = new WWW(backUrl);
        yield return frontWWW;
        yield return backWWW;
        deckDict.Add(id, new Tuple<Texture2D, Texture2D>(frontWWW.texture, backWWW.texture));
        callback(deckDict[id]);
    }

    public void GetCard(int cardID, int deckID, string frontUrl, string backUrl, Action<Tuple<Texture2D, Texture2D>> callback)
    {
        if (deckDict.ContainsKey(deckID))
        {
            Tuple<Texture2D, Texture2D> deckTextures = deckDict[deckID];            
            var frontTex = CropImageToCard(deckTextures.First, cardID);
            var backTex = deckTextures.Second;

            callback(new Tuple<Texture2D, Texture2D>(frontTex, backTex));
        } else
        {
            StartCoroutine(getDeck(deckID, frontUrl, backUrl, (deckTextures => {
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
