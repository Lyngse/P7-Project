
using SimpleJSON;

class WebSocketMessage : IJsonable
{
    MessageOptions options;
    IJsonable package;

    public WebSocketMessage(MessageOptions options, IJsonable package = null)
    {
        this.package = package;
        this.options = options;
    }

    public WebSocketMessage(JSONNode json)
    {
        fromJson(json);
    }

    public void fromJson(JSONNode json)
    {
        throw new System.NotImplementedException();
    }

    public JSONNode toJson()
    {
        var json = new JSONObject();
        json.Add("options", options.toJson());
        if(package != null)
            json.Add("package", package.toJson());
        return json;
    }
}
