using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
        
        // check ready list and if all players set canStartGame true
        internal string[] checkReadyList = new string[4];

        void Start()
        {
            instance = this;

            for(int i = 0; i < playerSections.Length; i++)
            {
                textList.Add(playerSections[i].playerText);
                imageList.Add(playerSections[i].roleImage);
                stateList.Add(playerSections[i].stateText);
            }
        }

        internal void checkList()
        {
            if(!checkReadyList.Contains("N"))
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
                case "hunter":
                    roleImageColor = Color.yellow;
                    break;

                case "lumberjack":
                    roleImageColor = Color.red;
                    break;

                case "builder":
                    roleImageColor = Color.gray;
                    break;

                case "doctor":
                    roleImageColor = Color.green;
                    break;
                
                default:
                    roleImageColor = Color.white;
                    break; 
            }

            instance.imageList[imgNo].color = roleImageColor;
        }
    
        internal static void setPlayerName(string value, int textNo = 0)
        {            
            if(UDPChat.instance.isServer)
            {
                if(value == "")
                {
                    UDPChat.instance.playerList.RemoveAt(textNo);
                    instance.textList[textNo].SetText(value);
                    
                    instance.nameArray[textNo] = value;
                }
                else
                {
                    UDPChat.instance.playerList.Add(value);
                    int x = UDPChat.instance.playerList.IndexOf(value);                   
                    instance.textList[x].SetText(value); 

                    instance.nameArray[x] = value;   
                }         
            }
            else
            {
                instance.textList[textNo].SetText(value);

                instance.nameArray[textNo] = value;
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

            instance.checkReadyList[stateNo] = value;
        }

        internal static void clearLobbyList()
        {
            for(int i = 0; i < instance.nameArray.Length; i++)
            {
                instance.textList[i].SetText(""); 
                instance.imageList[i].color = Color.white;
                instance.stateList[i].SetText("");

                instance.checkReadyList[i] = "";
            }
        }

        internal static void refreshLobbyList(int leftClientNo, int playerListLength)
        {
            setPlayerName("", leftClientNo);
            setRolePref("", leftClientNo);
            setReadyStatement("", leftClientNo);

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
                     
                    setReadyStatement(instance.checkReadyList[ x+1 ], x);                           
                } 
                
                // Reset empty playerSections
                int emptyCount = 4 -  (playerListLength - 1);

                switch(emptyCount)
                {
                    case 1:
                        instance.textList[3].SetText(""); 
                        instance.imageList[3].color = Color.white;
                        instance.stateList[3].SetText("");

                        instance.nameArray[3] = "";
                        instance.checkReadyList[3] = "";   
                        break;
                        
                    case 2:
                        instance.textList[2].SetText(""); 
                        instance.imageList[2].color = Color.white;
                        instance.stateList[2].SetText("");

                        instance.nameArray[2] = "";
                        instance.checkReadyList[2] = "";   

                        instance.textList[3].SetText(""); 
                        instance.imageList[3].color = Color.white;
                        instance.stateList[3].SetText("");

                        instance.nameArray[3] = "";
                        instance.checkReadyList[3] = "";   

                        break;    
                }
            }  
        }
    }
}