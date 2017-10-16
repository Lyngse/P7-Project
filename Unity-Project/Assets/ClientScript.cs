using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class ClientScript : MonoBehaviour {
    


    // Use this for initialization
    void Start () {
        var ws = new WebSocket("ws://localhost:5000");
        ws.Connect();
        ws.OnMessage += recieveMessage;
        string Json = JsonUtility.ToJson(new Vector2(3.0f, 5.0f));
       
        ws.SendAsync(Json, (success) => { });
      

    }
	
	// Update is called once per frame
	void Update () {

     
	}


    public void recieveMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);

        
    }




    


}
