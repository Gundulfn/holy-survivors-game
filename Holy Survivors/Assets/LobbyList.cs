using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace HD 
{
    public class LobbyList : MonoBehaviour
    {   
        public static LobbyList instance;
        
        // LobbyList Arrays
        public PlayerSection[] playerSections;

        internal List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();
        internal List<RawImage> imageList = new List<RawImage>();
        internal List<TextMeshProUGUI> stateList = new List<TextMeshProUGUI>();
        
        // string values of textList elements
        internal string[] nameArray = new string[4]{"", "", "", ""};
        // string values of players' rolenames
        internal string[] roleArray = new string[4]{"", "", "", ""};
        // ready array to check if all players set canStartGame true with "checkList" function
        internal string[] readyArray = new string[4];

        void Start()
        {
            instance = this;
   
            for(int i = 0; i < playerSections.Length; i++)
            {
                textList.Add(playerSections[i].playerText);
                imageList.Add(playerSections[i].roleImage);
                
                if(SceneManager.GetActiveScene().name == "MainScene")
                {
                    stateList.Add(playerSections[i].stateText);
                }
            }
        }

        internal void checkList()
        {
            if(!readyArray.Contains("N"))
            {
                MainSceneEventHandler.startGame();
            }
            else
            {
                // Do nothing
            }
        }

        internal static void setRolePref(string value, int imgNo = 0)
        {
            Color roleImageColor = Color.white;

            switch(value)
            {
                case "musketeer":
                    roleImageColor = Color.yellow;
                    break;

                case "lumberjack":
                    roleImageColor = Color.red;
                    break;

                case "pirate":
                    roleImageColor = Color.gray;
                    break;

                case "royalGuard":
                    roleImageColor = Color.blue;
                    break;
                
                default:
                    roleImageColor = Color.white;
                    break; 
            }

            if(!instance.playerSections[imgNo].gameObject.activeSelf)
            {
                instance.playerSections[imgNo].gameObject.SetActive(true);
            }

            instance.roleArray[imgNo] = value;
            instance.imageList[imgNo].color = roleImageColor;
        }
    
        internal static void setPlayerName(string value, int textNo = 0)
        {   
            if(!instance.playerSections[textNo].gameObject.activeSelf)
            {
                instance.playerSections[textNo].gameObject.SetActive(true);
            }

            if(UDPChat.instance.isServer)
            {
                if(value == "")
                {
                    if(SceneManager.GetActiveScene().name == "MainScene")
                    {
                        UDPChat.instance.playerList.RemoveAt(textNo);
                        instance.nameArray[textNo] = value;
                    }

                    instance.textList[textNo].SetText(value);
                }
                else
                {
                    if(SceneManager.GetActiveScene().name == "MainScene")
                    {
                        UDPChat.instance.playerList.Add(value);
                        int x = UDPChat.instance.playerList.IndexOf(value);         
                        instance.nameArray[x] = value;
                        instance.textList[x].SetText(value);
                    }
                    else
                    {
                        instance.textList[textNo].SetText(value);
                    }     
                }         
            }
            else
            {
                if(SceneManager.GetActiveScene().name == "MainScene")
                {
                    instance.nameArray[textNo] = value;
                }
                
                instance.textList[textNo].SetText(value);
            }
 
        }
    
        internal static void setReadyStatement(string value, int stateNo = 0)
        {
            if(value == "R")
            {
                instance.stateList[stateNo].SetText("R");
                instance.stateList[stateNo].color = Color.green;
            }
            else if(value == "N")
            {
                instance.stateList[stateNo].SetText("N");
                instance.stateList[stateNo].color = Color.white;
            }
            else
            {
                instance.stateList[stateNo].SetText("");
            } 

            instance.readyArray[stateNo] = value;
        }

        internal static void clearLobbyList()
        {
            for(int i = 0; i < instance.nameArray.Length; i++)
            {
                if(SceneManager.GetActiveScene().name == "MainScene")
                {
                    instance.stateList[i].SetText("");    
                }

                instance.textList[i].SetText(""); 
                instance.imageList[i].color = Color.white;

                instance.readyArray[i] = "";
                instance.roleArray[i] = "";
                instance.nameArray[i] = "";
            }
        }

        internal static void refreshLobbyList(int leftClientNo, int playerListLength)
        {
            setPlayerName("", leftClientNo);
            setRolePref("", leftClientNo);

            if(SceneManager.GetActiveScene().name == "MainScene")
            {
                setReadyStatement("", leftClientNo);
            }

            if(leftClientNo + 1 == playerListLength)
            {
                // Do nothing
            }
            else
            {
                // Set new clientNo
                UDPChat.clientNo -= 1;

                for(int x = leftClientNo; x < playerListLength - 1; x++)
                {                              
                    instance.textList[x].SetText(instance.nameArray[x + 1]); 
                    instance.imageList[x].color = instance.imageList[x + 1].color;
                    
                    if(SceneManager.GetActiveScene().name == "MainScene")
                    {
                        setReadyStatement(instance.readyArray[x + 1], x);   
                    }                           
                } 
                
                // Reset empty playerSections
                int emptyCount = 4 - (playerListLength - 1);

                switch(emptyCount)
                {
                    case 1:
                        instance.textList[3].SetText(""); 
                        instance.imageList[3].color = Color.white;  

                        if(SceneManager.GetActiveScene().name == "MainScene")
                        {
                            instance.stateList[3].SetText("");
                            
                            instance.nameArray[3] = "";
                            instance.roleArray[3] = "";
                            instance.readyArray[3] = ""; 
                        }

                        break;
                        
                    case 2:
                        instance.textList[2].SetText(""); 
                        instance.imageList[2].color = Color.white;   

                        instance.textList[3].SetText(""); 
                        instance.imageList[3].color = Color.white;
                        
                        if(SceneManager.GetActiveScene().name == "MainScene")
                        {
                            instance.stateList[2].SetText("");
                            instance.stateList[3].SetText("");

                            instance.nameArray[2] = "";
                            instance.roleArray[2] = "";
                            instance.readyArray[2] = "";
                            
                            instance.nameArray[3] = "";
                            instance.roleArray[3] = "";
                            instance.readyArray[3] = "";
                        }

                        break;    
                }
            }  
        }

        // When game starts to load GameScene, add rolenames to UDPChat.instance.playerList with usernames
        internal static void addRolesToPlayerList()
        {
            for(int i = 0; i < instance.nameArray.Length; i++)
            {
                if(instance.nameArray[i] != "")
                {
                    if(UDPChat.instance.isServer)
                    {
                        UDPChat.instance.playerList.Insert(i, instance.nameArray[i] + ";" + 
                                                              instance.roleArray[i]);
                        
                    }
                    else
                    {
                        UDPChat.instance.playerList.Add(instance.nameArray[i] + ";" + 
                                                        instance.roleArray[i]);
                    }
                }
                else
                {
                    // Do nothing
                }
            }
        }
    }
}