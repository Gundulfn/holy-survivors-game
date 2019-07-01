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

        internal TextMeshProUGUI[] textList = new TextMeshProUGUI[4];
        internal RawImage[] imageList = new RawImage[4];
        internal TextMeshProUGUI[] stateList = new TextMeshProUGUI[4];
        
        void Start()
        {
            instance = this;

            for(int i = 0; i < playerSections.Length; i++)
            {
                textList[i] = playerSections[i].playerText;
                imageList[i] = playerSections[i].roleImage;
                stateList[i] = playerSections[i].stateText;
            }
        }

        internal static void updatePlayerList()
        {
            for(int i = 0; i < UDPChat.instance.playerList.ToArray().Length; i++)
            {   
                instance.textList[i].SetText(UDPChat.instance.playerList[i]);  
            }
        }

        internal static void updateRoleList()
        {   
            for(int i = 0; i < UDPChat.instance.roleList.Length; i++)
            {   
                instance.imageList[i].color = colorByRoleName(UDPChat.instance.roleList[i]);
            }                 
        }

        internal static void updateStateList()
        { 
            for(int i = 0; i < UDPChat.instance.stateList.Length; i++)
            {   
                instance.stateList[i].SetText(UDPChat.instance.stateList[i]);
            }        
        }

        internal static void updateLobbyList()
        {
            updatePlayerList();
            updateRoleList();
            updateStateList();
        }

        internal static Color colorByRoleName(string roleName)
        {
            Color roleImageColor = Color.white;

            switch(roleName)
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

            return roleImageColor;
        }
    
        internal static void setValueByUsername(string username, string arrayName, 
                                                    string value)
        {
            int i = UDPChat.instance.playerList.IndexOf(username);

            if(arrayName == "roleList")
            {
                UDPChat.instance.roleList[i] = value;

                MainSceneEventHandler.instance.countDownText.text += "roleList value: " + value;
                LobbyList.updateRoleList();
            }
            else if(arrayName == "stateList")
            {
                UDPChat.instance.stateList[i] = value;
                
                MainSceneEventHandler.instance.countDownText.text += "stateList value: " + value;
                LobbyList.updateStateList();
            }
            else
            {
                MainSceneEventHandler.instance.countDownText.text += "Array name is not defined";
            }
        }
    }
}
