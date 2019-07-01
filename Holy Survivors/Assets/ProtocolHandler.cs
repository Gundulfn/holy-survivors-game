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
                    case ProtocolLabels.joinGame:

                        UDPChat.instance.playerList.Add(sections[1]);
                        LobbyList.updatePlayerList();
                        
                        // For new client
                        // server will send 3 info messages to new client
                        // info messages includes usernames, role selections and ready statements
                        // info message's second element is added for setting list type

                        listChanged("playerList", ipEndpoint);
                        listChanged("roleList", ipEndpoint);
                        listChanged("stateList", ipEndpoint);  

                        // For older clients
                        // object[] nameMsg = new object[2]{ProtocolLabels.newClient, sections[1]};

                        // string newClientMsg = MessageMaker.makeMessage(nameMsg);
                        // UDPChat.instance.Send(newClientMsg);
                        break; 

                    case ProtocolLabels.roleSelected:

                        LobbyList.setValueByUsername(sections[1], 
                                                     "roleList", 
                                                     sections[2]);
                        LobbyList.updateRoleList();

                        UDPChat.instance.Send(message);
                        break;

                    case ProtocolLabels.clientLeft:

                        UDPChat.instance.playerList.Remove(sections[1]);
                        UDPChat.RemoveClient(ipEndpoint);

                        LobbyList.setValueByUsername(sections[1], 
                                                     "roleList", 
                                                     "");

                        LobbyList.setValueByUsername(sections[1], 
                                                     "stateList", 
                                                     "");
                        LobbyList.updateLobbyList();
                        
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
                    case ProtocolLabels.lobbyListInfo:

                        for(int i = 0; i < sections.Length; i++)
                        {
                            MainSceneEventHandler.instance.countDownText.text += sections[i] + " ";
                        }
                        break;
                    
                    case ProtocolLabels.newClient:

                        UDPChat.instance.playerList.Add(sections[1]);
                        LobbyList.updatePlayerList();
                        break;

                    case ProtocolLabels.roleSelected:

                        LobbyList.setValueByUsername(sections[1], 
                                                     "roleList", 
                                                     sections[2]);
                        break;

                    case ProtocolLabels.clientLeft:

                        UDPChat.instance.playerList.Remove(sections[1]);

                        LobbyList.setValueByUsername(sections[1], 
                                                     "roleList", 
                                                     "");

                        LobbyList.setValueByUsername(sections[1], 
                                                     "stateList", 
                                                     "");
                        LobbyList.updateLobbyList();
                        break;

                    default:      
                        break;  
                }       
            }
        }

            internal static void listChanged(string listType, IPEndPoint ipEndpoint)
            {
                object[] updateMsg = new object[6];

                updateMsg[0] = ProtocolLabels.lobbyListInfo;
                updateMsg[1] = "playerList";

                switch(listType)
                {
                    case "playerList":
                        updateMsg = new object[2 + UDPChat.instance.playerList.ToArray().Length];

                        for(int i = 0; i < UDPChat.instance.playerList.ToArray().Length; i++)
                        {
                            // Runtime debug for now
                            MainSceneEventHandler.instance.countDownText.text += "player ";
                            
                            updateMsg[i + 2] = UDPChat.instance.playerList[i];                          
                        }

                        break;
                    case "roleList":
                        updateMsg = new object[2 + UDPChat.instance.roleList.Length];

                        for(int i = 0; i < UDPChat.instance.roleList.Length; i++)
                        {
                            // Runtime debug for now
                            MainSceneEventHandler.instance.countDownText.text += "role ";
                            
                            updateMsg[i + 2] = UDPChat.instance.roleList[i];                          
                        }

                        break;
                    case "stateList":
                        updateMsg = new object[2 + UDPChat.instance.stateList.Length];

                        for(int i = 0; i < UDPChat.instance.stateList.Length; i++)
                        {
                            // Runtime debug for now
                            MainSceneEventHandler.instance.countDownText.text += "state ";
                            
                            updateMsg[i + 2] = UDPChat.instance.stateList[i];                          
                        }

                        break;
                         
                }                      
                
                string response = MessageMaker.makeMessage(updateMsg);

                UDPChat.instance.connection.Send(response, ipEndpoint);  
            }
    }
}    
