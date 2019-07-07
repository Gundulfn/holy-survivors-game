using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using TMPro;

namespace HD
{
  public class UdpConnectedClient
  {
    readonly UdpClient connection;
    
    public UdpConnectedClient(IPAddress ip = null)
    {
      if(UDPChat.instance.isServer)
      {
        connection = new UdpClient(Globals.port);
      }
      else
      {
        connection = new UdpClient(); // Auto-bind port
      }
     
      connection.BeginReceive(OnReceive, null);
    }

    public void Close()
    {
      connection.Close();
    }

    void OnReceive(IAsyncResult ar)
    { 
      try
      {
        IPEndPoint ipEndpoint = null;
        byte[] data = connection.EndReceive(ar, ref ipEndpoint);
        
        UDPChat.AddClient(ipEndpoint);
        
        string message = System.Text.Encoding.UTF8.GetString(data);
        
        //Message Processes according to messageType      
        ProtocolHandler.Handle(message, ipEndpoint);
        
        if(UDPChat.instance.isServer)
        {
          UDPChat.BroadcastChatMessage(message, ipEndpoint);
        }

        UDPChat.instance.connectionFailed = false;
      }
      catch(SocketException e)
      { 
        Debug.Log(e);

        UDPChat.instance.connectionFailed = true;
      }

      connection.BeginReceive(OnReceive, null);
    }

    internal void Send(string message, IPEndPoint ipEndpoint)
    {
      byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
      connection.Send(data, data.Length, ipEndpoint);
      Debug.Log("Message: " + message);
    }
  }
}