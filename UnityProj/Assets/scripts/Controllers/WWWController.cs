using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WWWController: MonoBehaviour
{
    public Dictionary<int, Tuple<Texture2D, Texture2D>> deckDict = new Dictionary<int, Tuple<Texture2D, Texture2D>>();

    public IEnumerator getDeck(int id, string frontUrl, string backUrl, Action callback)
    {
        WWW frontWWW = new WWW(frontUrl);
        WWW backWWW = new WWW(backUrl);
        yield return frontWWW;
        yield return backWWW;
        deckDict.Add(id, new Tuple<Texture2D, Texture2D>(frontWWW.texture, backWWW.texture));
        callback();
    }

}
