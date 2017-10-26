
using SimpleJSON;

class toClientMessage : IJsonable
{
    public string code;
    public Utility.ClientColor clientColor;
    public IJsonable message;

    public void fromJson(string jsonString)
    {
        throw new System.NotImplementedException();
    }

    public JSONNode toJson()
    {
        var json = new JSONObject();
        json.Add("code", new JSONString(code));
        json.Add("clientColor", new JSONNumber((int)clientColor));
        var m = message.toJson();
        json.Add("message", m);
        return json;
    }
}
