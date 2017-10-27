using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

class StringPackage : IJsonable
{
    public string package;

    public StringPackage(string package)
    {
        this.package = package;
    }

    public StringPackage(JSONNode json)
    {
        fromJson(json);
    }

    public void fromJson(JSONNode json)
    {
        package = json.Value;
    }

    public JSONNode toJson()
    {

        return new JSONString(package);
    }
}
