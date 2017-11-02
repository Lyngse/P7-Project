
using SimpleJSON;

class MessageOptions : IJsonable
{
    public string type;
    public string code;
    public Utility.ClientColor color = Utility.ClientColor.none;
    public string packageType;

    public MessageOptions(string type, string code = null, Utility.ClientColor color = Utility.ClientColor.none, string packageType = null)
    {
        this.type = type;
        this.code = code;
        this.color = color;
        this.packageType = packageType;
    }

    public MessageOptions(JSONNode json)
    {
        fromJson(json);
    }

    public void fromJson(JSONNode json)
    {
        type = json["type"].Value;
        if(json["code"] != null)
            code = json["code"].Value;
        if (json["color"] != null)
        {
            var colorInt = json["color"].AsInt;
            color = (Utility.ClientColor)colorInt;
        }
        if(json["packageType"] != null)
        {
            packageType = json["packageType"].Value;
        }
    }

    public JSONNode toJson()
    {
        var json = new JSONObject();
        json.Add("type", new JSONString(type));
        if(code != null)
            json.Add("code", new JSONString(code));
        if (color != Utility.ClientColor.none)
            json.Add("color", new JSONNumber((int)color));
        if (packageType != null)
            json.Add("packageType", new JSONString(packageType));
        
        return json;
    }
}
