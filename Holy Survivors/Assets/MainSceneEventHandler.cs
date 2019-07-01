using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HD
{
    public class MainSceneEventHandler : MonoBehaviour
    {
        // Existed UDP Gameobject and Prefab
        public GameObject udp;
        public GameObject udpPrefab;
        
        // For static calls
        public static MainSceneEventHandler instance;
        
        // Main UI Elements
        private GameObject mainUI;
        private GameObject lobbyUI;

        // Special UI Element
        public GameObject lobbyGameButton;
        public TextMeshProUGUI countDownText;

        // InputField Settings
        public InputField usernameInput;
        private string username;
        public InputField ipInput;
        private string ipText;

        void Start()
        {
            instance = this;
            mainUI = gameObject.transform.GetChild(0).gameObject;
            lobbyUI = gameObject.transform.GetChild(1).gameObject;
        }

        void Update()
        {
            // Checking UDP gameObject activity to set UI
            if(udp.activeSelf)
            {
                if(UDPChat.instance.connectionFailed)
                {
                    lobbyUI.SetActive(false);
                    mainUI.SetActive(true);

                    resetUDP();
                }
                else
                {
                    lobbyUI.SetActive(true);
                    mainUI.SetActive(false);
                }
            }
        }

        // OnClick Functions
        public void clientButtonFunc()
        {
            ipText = ipInput.text;
            username = usernameInput.text;

            if(checkUserName(username)){
                Globals.isServer = false;

                IPAddress ip = IPAddress.Parse(ipText);
                udp.GetComponent<UDPChat>().serverIp = ip;
                udp.SetActive(true);
                
            }
        }

        public void serverButtonFunc()
        {
            username = usernameInput.text;

            if(checkUserName(username)){
                Globals.isServer = true;
                udp.SetActive(true);
                UDPChat.instance.connectionFailed = false;
                
                lobbyGameButton.SetActive(true);
            }     
        }
        
        public void exitButtonFunc()
        {
            // Set UDP settings as default
            if(!UDPChat.instance.isServer)
            {
                UDPChat.instance.clientDisconnected();
            }

            UDPChat.instance.connection.Close();
            resetUDP();
            
            // Set UI elements to show Main Menu
            lobbyUI.SetActive(false);
            lobbyGameButton.SetActive(false);
            mainUI.SetActive(true);
        }

        public void startGameButtonFunc()
        {}
       
        public void stopGameButtonFunc()
        {}

        public void roleButtonFunc(string roleName)
        {
            object[] roleInfo = new object[3] {ProtocolLabels.roleSelected, 
                                               UDPChat.instance.username, 
                                               roleName};

            string msg = MessageMaker.makeMessage(roleInfo);
            
            if(UDPChat.instance.isServer)
            {
                // Server must set them by itself
                UDPChat.instance.roleList[0] = roleName;
                LobbyList.updateRoleList();

                UDPChat.instance.Send(msg);
            }
            else
            {
                UDPChat.instance.connection.Send(msg,
                                            new IPEndPoint(UDPChat.instance.serverIp, Globals.port));
            }
        }

        // Common Functions in this script
        
        /// To check username if it is approative or not
        /// Username cannot be space like " "   
        private bool isUsernameApproative;

        private bool checkUserName(string username)
        {
            int nameLength = username.Length;

            for(int i = 0; i < nameLength; i++)
            {
                if(username[i] == ' ')
                {
                    isUsernameApproative = false;
                }
                else
                {
                    isUsernameApproative = true;
                }
            }

            return isUsernameApproative;
        }

        /// set "udp" a new gameobject with "udpPrefab"
        private void resetUDP()
        {
            Destroy(udp);
            udp = Instantiate(udpPrefab);
            udp.name = "UDP GameObject";
        }
    
    }    
}
