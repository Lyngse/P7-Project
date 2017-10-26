using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

class StringMessage : IJsonable
{
    private string message;

    public StringMessage(string message)
    {
        this.message = message;
    }

    public void fromJson(string jsonString)
    {
        message = jsonString;
    }

    public JSONNode toJson()
    {

        return new JSONString(message);
    }
}
