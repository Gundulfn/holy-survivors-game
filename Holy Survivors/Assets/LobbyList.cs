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
        private string[] nameArray = new string[4]{"", "", "", ""};
        
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
            else
            {
                instance.stateList[stateNo].SetText("N");
                instance.stateList[stateNo].color = Color.white;
            } 
        }

        internal static void clearLobbyList()
        {
            for(int i = 0; i < 4; i++)
            {
                setPlayerName("", i);
                setRolePref("", i);
                setReadyStatement("", i);
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
                    instance.stateList[x].SetText("");                             
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
                        break;
                        
                    case 2:
                        instance.textList[2].SetText(""); 
                        instance.imageList[2].color = Color.white;
                        instance.stateList[2].SetText("");

                        instance.nameArray[2] = "";

                        instance.textList[3].SetText(""); 
                        instance.imageList[3].color = Color.white;
                        instance.stateList[3].SetText("");

                        instance.nameArray[3] = "";
                        break;    
                }
            }  
        }
    }
}
