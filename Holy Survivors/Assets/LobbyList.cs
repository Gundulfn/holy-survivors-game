using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        
        internal List<string> nameList = new List<string>();
        
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

        internal static void setRolePref(string value, string username)
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

            int imgNo = UDPChat.instance.playerList.IndexOf(username);
            instance.imageList[imgNo].color = roleImageColor;
        }
    
        internal static void setPlayerName(string value)
        {
            UDPChat.instance.playerList.Add(value);

            int textNo = UDPChat.instance.playerList.IndexOf(value);
            
            instance.textList.ToArray()[textNo].SetText(value);
        }
    
        internal static void setReadyStatement(bool readyState, string username)
        {
            int stateNo = UDPChat.instance.playerList.IndexOf(username);

            if(readyState)
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
    }
}
