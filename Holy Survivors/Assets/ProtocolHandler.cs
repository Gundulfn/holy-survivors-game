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
            Debug.Log("Handler: " + message);

            if(UDPChat.instance.isServer)
            {
                switch(messageType)
                {
                    case ProtocolLabels.joinRequest:

                        if(UDPChat.instance.playerList.ToArray().Length == 4 || 
                           UDPChat.instance.gameState == "start")
                        {
                            object[] rejectMsg = new object[2]{ ProtocolLabels.joinRequest, "rejected"};

                            string rejMsg = MessageMaker.makeMessage(rejectMsg);
                            UDPChat.instance.connection.Send(rejMsg, ipEndpoint);
                        }
                        else
                        {
                        // Add player informations to lobby
                        LobbyList.setPlayerName(sections[1]);

                        int newClientNo = UDPChat.instance.playerList.IndexOf(sections[1]);
                        LobbyList.setReadyStatement("N", newClientNo);
                        
                        // give info about itself to client to update it
                        // and set its clientInfo
       
                        object[] nameMsg = new object[5]{ ProtocolLabels.clientInfo, 
                                                          UDPChat.instance.username, 
                                                          newClientNo,
                                                          UDPChat.instance.roleName,
                                                          UDPChat.instance.readyStatement
                                                          };

                        string infoMsg = MessageMaker.makeMessage(nameMsg);
                        UDPChat.instance.connection.Send(infoMsg, ipEndpoint);
                        }
                        
                        break; 

                    case ProtocolLabels.roleSelected:

                        LobbyList.setRolePref(sections[2], System.Int32.Parse(sections[1]));             

                        UDPChat.instance.Send(message);
                        break;

                    case ProtocolLabels.clientReady:
                        
                        LobbyList.setReadyStatement(sections[2], System.Int32.Parse(sections[1]));
                        break;
                    
                    case ProtocolLabels.clientLeft:
                        
                        int leftClientNo = System.Int32.Parse(sections[1]);
                        int playerListLength = UDPChat.instance.playerList.ToArray().Length;
                        
                        LobbyList.refreshLobbyList(leftClientNo, playerListLength);                        
                        
                        UDPChat.RemoveClient(ipEndpoint);
                        
                        MainSceneEventHandler.stopGame();

                        // send this message to inform other clients
                        object[] exitMsgParts = new object[3]{ProtocolLabels.clientLeft, sections[1], 
                                                                playerListLength};
      
                        string exitMsg = MessageMaker.makeMessage(exitMsgParts);

                        UDPChat.instance.Send(exitMsg);
                        break;
                   
                    case ProtocolLabels.playerMove:
                        float[] coordinates = new float[3]{float.Parse(sections[2]),
                                                           float.Parse(sections[3]),
                                                           float.Parse(sections[4])};
                                                           
                        GameSceneEventHandler.movePlayerObj(System.Int32.Parse(sections[1]), coordinates);
                        break;
                    
                    case ProtocolLabels.playerRot:

                        GameSceneEventHandler.rotatePlayerObj(System.Int32.Parse(sections[1]),
                                                              float.Parse(sections[2]),
                                                              float.Parse(sections[3]));
                        break;

                    default:
                        break;  
                }
            }
            else
            {   
                switch(messageType)
                {
                    case ProtocolLabels.joinRequest:    
                        // means you cannot join lobby, set UI normal
                        MainSceneEventHandler.instance.exitButtonFunc();
                        break;

                    case ProtocolLabels.clientInfo:

                        if(UDPChat.clientNo == 0)
                        {
                            //add server
                            LobbyList.setPlayerName(sections[1]);
                            LobbyList.setRolePref(sections[3]);
                            LobbyList.setReadyStatement(sections[4]);
                            
                            //client settings
                            UDPChat.clientNo = System.Int32.Parse(sections[2]);
                            
                            LobbyList.setPlayerName(UDPChat.instance.username, 
                                                    UDPChat.clientNo);  

                            LobbyList.setReadyStatement("N", UDPChat.clientNo);                                         
                            
                            object[] clientMsg = new object[3]{ProtocolLabels.newClient, 
                                                               UDPChat.instance.username,
                                                               UDPChat.clientNo};

                            string othersMsg = MessageMaker.makeMessage(clientMsg);
                            UDPChat.instance.Send(othersMsg);                        
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
                                                         UDPChat.instance.roleName,
                                                         UDPChat.instance.readyStatement};

                        string infoMsg = MessageMaker.makeMessage(nameMsg);
                        UDPChat.instance.connection.Send(infoMsg, ipEndpoint);
                        break;

                    case ProtocolLabels.roleSelected:
                        
                        LobbyList.setRolePref(sections[2], System.Int32.Parse(sections[1]));
                        break;

                    case ProtocolLabels.clientReady:
                        
                        LobbyList.setReadyStatement(sections[2], System.Int32.Parse(sections[1]));
                        break;

                    case ProtocolLabels.clientLeft:

                        int leftClientNo = System.Int32.Parse(sections[1]);
                        
                        LobbyList.refreshLobbyList(leftClientNo, System.Int32.Parse(sections[2]));
                        
                        MainSceneEventHandler.stopGame(); // To stop countdown
                        break;

                    case ProtocolLabels.gameAction:
                        UDPChat.instance.gameState = sections[1];
                        
                        if(UDPChat.instance.gameState == "start")
                        {
                            MainSceneEventHandler.startGame();
                        }
                        else
                        {
                            MainSceneEventHandler.stopGame();
                        }
                        
                        break;

                    case ProtocolLabels.playerMove:
                        float[] coordinates = new float[3]{float.Parse(sections[2]),
                                                           float.Parse(sections[3]),
                                                           float.Parse(sections[4])};
                                                           
                        GameSceneEventHandler.movePlayerObj(System.Int32.Parse(sections[1]), coordinates);
                        
                        break;

                    case ProtocolLabels.playerRot:

                        GameSceneEventHandler.rotatePlayerObj(System.Int32.Parse(sections[1]),
                                                              float.Parse(sections[2]),
                                                              float.Parse(sections[3]));
                        break;

                    default:      
                        break;  
                }       
            }
        }
    }
}    