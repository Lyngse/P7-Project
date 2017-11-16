using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

class ColorChangePackage : IJsonable
{
    Utility.ClientColor fromColor;
    Utility.ClientColor toColor;

    public ColorChangePackage(Utility.ClientColor fromColor, Utility.ClientColor toColor)
    {
        this.fromColor = fromColor;
        this.toColor = toColor;
    }

    public ColorChangePackage(JSONNode json)
    {
        fromJson(json);
    }

    public void fromJson(JSONNode json)
    {
        
    }

    public JSONNode toJson()
    {
        throw new NotImplementedException();
    }
}
