using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using TMPro;

namespace HD
{
  public class UDPChat : MonoBehaviour
  {
    // Script references
    public static UDPChat instance;
    public UdpConnectedClient connection;
    // UDP statement
    public IPAddress serverIp;
    public bool isServer;

    public bool connectionFailed;
    
    // For connection, protocol messages and more
    internal List<IPEndPoint> clientList = new List<IPEndPoint>();

    internal static int clientNo = 0;
    
    internal List<string> playerList = new List<string>();
    internal string roleName = "";
    internal string readyStatement = "N";

    internal string gameState = "stop";
    
    // Username Settings
    internal string username;
    
    public void Awake()
    {
      instance = this;
      username = MainSceneEventHandler.instance.usernameInput.text;

      if(serverIp == null)
      {
        this.isServer = true;
        connection = new UdpConnectedClient();

        LobbyList.setPlayerName(username);
        LobbyList.setReadyStatement("N");
      }
      else
      {
        connection = new UdpConnectedClient(ip: serverIp);
        AddClient(new IPEndPoint(serverIp, Globals.port)); 
      }
    }

    internal static void AddClient(
      IPEndPoint ipEndpoint)
    {
     
      if(instance.clientList.Contains(ipEndpoint) == false && !instance.connectionFailed)
      {
        UnityEngine.MonoBehaviour.print($"Connect to {ipEndpoint}");
        instance.clientList.Add(ipEndpoint);
        
        // Request Message to Join Lobby      
        if(!instance.isServer)
        {
          object[] req = new object[2]{ProtocolLabels.joinRequest, instance.username};
          string message = MessageMaker.makeMessage(req);
          instance.connection.Send(message, ipEndpoint);
        }
        
      }
    }

    internal static void RemoveClient(
      IPEndPoint ipEndpoint)
    { 
      instance.clientList.Remove(ipEndpoint);
    }

    void OnApplicationQuit()
    { 
      if(instance.isServer){
        // Do nothing
      }else{
        clientDisconnected();
      }
      
      connection.Close();
    }

    public void Send(
      string message, IPEndPoint source = null)
    {
      BroadcastChatMessage(message, source);
    }

    internal static void BroadcastChatMessage(string message, IPEndPoint source)
    {
      foreach(var ip in instance.clientList)
      {
        if (source == null || ip != source) {
          instance.connection.Send(message, ip);
        }
      }
    }
    
    internal void clientDisconnected(){
      if(clientNo != 0)
      {
        object[] exitMsgParts = new object[2]{ProtocolLabels.clientLeft, clientNo};
      
        string exitMsg = MessageMaker.makeMessage(exitMsgParts);

        instance.connection.Send(exitMsg, new IPEndPoint(instance.serverIp, Globals.port));

        clientNo = 0;
      }
    }

  }
}