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

                        object[] updateMsg = new object[2 + UDPChat.instance.playerList.ToArray().Length];

                        updateMsg[0] = ProtocolLabels.lobbyListInfo;
                        updateMsg[1] = "playerList";

                        // Runtime debug for now
                        MainSceneEventHandler.instance.countDownText.text += 
                                                            UDPChat.instance.playerList.ToArray().Length;

                        for(int i = 0; i < 4; i++)
                        {
                            // Runtime debug for now
                            MainSceneEventHandler.instance.countDownText.text += 
                            UDPChat.instance.playerList[i] + " ";

                            updateMsg[i + 2] = UDPChat.instance.playerList[i];                          
                        }      
                        
                        string lobbyInfoMsg = MessageMaker.makeMessage(updateMsg);

                        UDPChat.instance.connection.Send(lobbyInfoMsg, ipEndpoint);  

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
                            MainSceneEventHandler.instance.countDownText.text += sections[i];
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

            // internal void listChanged(string listType)
            // {
            //     // updatePlayerList MainSceneEventHandler.updateLobbyList();
                
            //     string response = "";
                
            //     object[] newList = new object[1 + UDPChat.instance.playerList.ToArray().Length];

            //     newList.SetValue("L1", 0);

            //     for(int i = 1; i < newList.Length; i++)
            //     {
            //         newList.SetValue(UDPChat.instance.playerList[i - 1], i);
            //     }

            //     response = MessageMaker.makeMessage(newList);

            //     UDPChat.instance.Send(response);
            // }
    }
}    
