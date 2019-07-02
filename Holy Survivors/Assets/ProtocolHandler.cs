using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace HD
{
    public class ProtocolHandler
    {
        public static void Handle(string message, IPEndPoint ipEndpoint)
        {         
            string[] sections = message.Split(';');
            string messageType = sections[0];
        
            if(UDPChat.instance.isServer)
            {
                switch(messageType)
                {
                    case ProtocolLabels.newClient:

                        UDPChat.instance.playerList.Add(sections[1]);
                        LobbyList.setPlayerName(sections[1]);

                        // give info about itself to client to update it

                        object[] nameMsg = new object[2]{ProtocolLabels.clientInfo, UDPChat.instance.username};

                        string infoMsg = MessageMaker.makeMessage(nameMsg);
                        UDPChat.instance.connection.Send(infoMsg, ipEndpoint);
                        break; 

                    case ProtocolLabels.roleSelected:

                        LobbyList.setRolePref(sections[2], sections[1]);             

                        UDPChat.instance.Send(message);
                        break;

                    case ProtocolLabels.clientLeft:

                        UDPChat.instance.playerList.Remove(sections[1]);
                        UDPChat.RemoveClient(ipEndpoint);

                    
                        
                        UDPChat.instance.Send(message);
                        break;

                    default:
                        break;  
                }
            }
            else
            {   
                switch(messageType)
                {
                    case ProtocolLabels.clientInfo:

                        UDPChat.instance.playerList.Add(sections[1]);
                        LobbyList.setPlayerName(sections[1]);

                        break;
                    
                    case ProtocolLabels.newClient:

                        UDPChat.instance.playerList.Add(sections[1]);
                        LobbyList.setPlayerName(sections[1]);

                        object[] nameMsg = new object[2]{ProtocolLabels.clientInfo, UDPChat.instance.username};

                        string infoMsg = MessageMaker.makeMessage(nameMsg);
                        UDPChat.instance.connection.Send(infoMsg, ipEndpoint);
                        break;

                    case ProtocolLabels.roleSelected:

                   
                        break;

                    case ProtocolLabels.clientLeft:

                        UDPChat.instance.playerList.Remove(sections[1]);

                        break;

                    default:      
                        break;  
                }       
            }
        }
    }
}    
