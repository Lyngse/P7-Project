using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Utility
{
    public enum ClientColor { none, red, green, blue, white, black, yellow, orange, purple };
    public enum websocketEvent { Open, Message, Close };

    public static Color[] colors = { Color.clear, Color.red, Color.green, Color.blue, Color.white, Color.black, Color.yellow, new Color(1, 0.7f, 0), new Color(0.8f, 0, 1) };
}