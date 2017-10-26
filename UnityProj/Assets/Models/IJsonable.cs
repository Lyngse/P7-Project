using SimpleJSON;

interface IJsonable
{
    void fromJson(string jsonString);

    JSONNode toJson();
}
