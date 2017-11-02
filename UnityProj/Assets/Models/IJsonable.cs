using SimpleJSON;

interface IJsonable
{

    void fromJson(JSONNode json);

    JSONNode toJson();
}
