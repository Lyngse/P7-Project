
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

public class NetworkScript : MonoBehaviour {

  public Button hostButton;
  public Button connectButton;
  int hostId;
  int reliableChannelId;
  int hostPort = 11000;
  bool isServer;

  enum colors { red, green, blue, white, black, yellow, orange, purple };

  int[] connectionColors = { 0, 0, 0, 0, 0, 0, 0, 0 };

	// Use this for initialization
	void Start () {
    hostButton.onClick.AddListener(onHostButtonClick);
    connectButton.onClick.AddListener(onConnectButtonClick);
    hostId = -1;
  }

  // Update is called once per frame
  void Update () {
    if(hostId > -1)
    {
      networkStatus status = getNetworkStatus();
      if (isServer)
      {
        serverHandleStatus(status);
      }
      else
      {
        clientHandleStatus(status);
      }
    }
  }

  void serverHandleStatus(networkStatus status)
  {
    switch (status.eventType)
    {
      case NetworkEventType.ConnectEvent:
        Debug.Log("server connectionEvent: " + status.connectionId);
        send(, status.connectionId);
        IpEndPoint ipEndPoint = GetIpEndPoint(status.connectionId);
        Debug.Log("Ip: " + ipEndPoint.address + ":" + ipEndPoint.port);
        break;
      case NetworkEventType.DataEvent:
        Debug.Log("server dataEvent: " + status.connectionId + " message: " + status.message);
        break;
      case NetworkEventType.DisconnectEvent:
        Debug.Log("server disconnectionEvent: " + status.connectionId);
        break;
      case NetworkEventType.BroadcastEvent:
        Debug.Log("server broadcastEvent: " + status.connectionId);
        break;
    }
  }

  void clientHandleStatus(networkStatus status)
  {
    switch (status.eventType)
    {
      case NetworkEventType.ConnectEvent:
        Debug.Log("client connectionEvent: " + status.connectionId);
        break;
      case NetworkEventType.DataEvent:
        Debug.Log("client dataEvent: " + status.connectionId + " message: " + status.message);
        break;
      case NetworkEventType.DisconnectEvent:
        Debug.Log("client disconnectionEvent: " + status.connectionId);
        break;
      case NetworkEventType.BroadcastEvent:
        Debug.Log("client broadcastEvent: " + status.connectionId);
        break;
    }
  }

  void send(string message, int connectionId)
  {
    byte[] bytes = Encoding.ASCII.GetBytes(message);
    byte error;
    NetworkTransport.Send(hostId, connectionId, 0, bytes, bytes.Length, out error);
  }

  void send(object obj, int connectionId)
  {
    var json = JsonUtility.ToJson(obj);
    send(json, connectionId);
  }

  void onHostButtonClick()
  {
    if (hostId == -1)
    {
      isServer = true;
      startNetwork();
    }
  }

  void onConnectButtonClick()
  {
    if (hostId == -1)
    {
      isServer = false;
      startNetwork();
    }
  }

  
  void startNetwork()
  {
    NetworkTransport.Init();
    ConnectionConfig config = getConfig();
    HostTopology topology = new HostTopology(config, 8);
    if (isServer)
    {
      hostId = NetworkTransport.AddHost(topology, hostPort);
      Debug.Log("socket open on " + hostPort);
    }
    else
    {
      hostId = NetworkTransport.AddHost(topology);
      Debug.Log("socket open on random port");
    }
  }


  ConnectionConfig getConfig()
  {
    ConnectionConfig config = new ConnectionConfig();
    reliableChannelId = config.AddChannel(QosType.Reliable);
    config.DisconnectTimeout = 5000;
    return config;
  }

  networkStatus getNetworkStatus()
  {
    int connectionId;
    int channelId;
    byte[] recBuffer = new byte[1024];
    int bufferSize = 1024;
    int dataSize;
    byte error;
    NetworkEventType recData = NetworkTransport.ReceiveFromHost(hostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
    string message = Encoding.ASCII.GetString(recBuffer);
    return new networkStatus(connectionId, message, recData);
  }

  IpEndPoint GetIpEndPoint(int connectionId)
  {
    int port;
    string address;
    NetworkID network;
    NodeID dstNode;
    byte error;
    NetworkTransport.GetConnectionInfo(hostId, connectionId, out address, out port, out network, out dstNode, out error);
    return new IpEndPoint(address, port);
  }

  class networkStatus
  {
    public int connectionId;
    public string message;
    public NetworkEventType eventType;

    public networkStatus(int connectionId, string message, NetworkEventType eventType)
    {
      this.connectionId = connectionId;
      this.message = message;
      this.eventType = eventType;
    }
  }

  class IpEndPoint
  {
    public string address;
    public int port;

    public IpEndPoint(string add, int p)
    {
      address = add;
      port = p;
    }

  }
}




