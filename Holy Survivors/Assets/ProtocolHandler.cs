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
                    case ProtocolLabels.joinRequest:

                        LobbyList.setPlayerName(sections[1]);

                        // give info about itself to client to update it
                        // and set its clientInfo
                        MainSceneEventHandler.instance.countDownText.text += UDPChat.instance.roleList[0] +
                                                                             " " + UDPChat.instance.stateList[0];
                        object[] nameMsg = new object[5]{ ProtocolLabels.clientInfo, 
                                                          UDPChat.instance.username, 
                                                          UDPChat.instance.playerList.IndexOf(sections[1]),
                                                          UDPChat.instance.roleList[0],
                                                          UDPChat.instance.stateList[0]
                                                          };

                        string infoMsg = MessageMaker.makeMessage(nameMsg);
                        UDPChat.instance.connection.Send(infoMsg, ipEndpoint);
                        break; 

                    case ProtocolLabels.roleSelected:

                        LobbyList.setRolePref(sections[2], System.Int32.Parse(sections[1]));             

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
                        
                        if(UDPChat.clientNo == 0)
                        {
                            //add server
                            LobbyList.setPlayerName(sections[1]);
                            LobbyList.setRolePref(sections[3]);
                            LobbyList.setReadyStatement(sections[4]);

                            UDPChat.clientNo = System.Int32.Parse(sections[2]);
                            
                            LobbyList.setPlayerName(UDPChat.instance.username, 
                                                    UDPChat.clientNo);                  
                            
                            // object[] clientMsg = new object[3]{ProtocolLabels.newClient, 
                            //                                    UDPChat.instance.username,
                            //                                    UDPChat.clientNo};

                            // string othersMsg = MessageMaker.makeMessage(clientMsg);
                            // UDPChat.instance.Send(othersMsg);                        
                        }
                        else
                        {
                            LobbyList.setPlayerName(sections[1], System.Int32.Parse(sections[2]));
                            LobbyList.setRolePref(sections[3], System.Int32.Parse(sections[2]));
                            LobbyList.setReadyStatement(sections[4], System.Int32.Parse(sections[2]));
                        }                        

                        break;
                    
                    case ProtocolLabels.newClient:
                    
                        LobbyList.setPlayerName(sections[1], System.Int32.Parse(sections[2]));

                        object[] nameMsg = new object[5]{ProtocolLabels.clientInfo, 
                                                         UDPChat.instance.username,
                                                         UDPChat.clientNo,
                                                         UDPChat.instance.roleList[UDPChat.clientNo],
                                                         UDPChat.instance.stateList[UDPChat.clientNo]};

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
