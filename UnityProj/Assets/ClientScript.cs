using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using WebSocketSharp;

class ClientScript : NetworkScript
{
    public Button clientButton;
    public InputField codeInput;

    private void Start()
    {
        clientButton.onClick.AddListener(clientClick);
    }

    void clientClick()
    {
        startNetwork();
    }

    protected override void onMessage(string data)
    {
        throw new NotImplementedException();
    }

    protected override void onOpen()
    {
        string hostCode = codeInput.text;
        var message = createMessage("client_connection", hostCode);
        webSocket.Send(message.ToString());
    }

    void sendToHost(IJsonable message)
    {
        throw new NotImplementedException();
    }
}