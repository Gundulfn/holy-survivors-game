using System.Linq;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public GameObject startButton;
        public TextMeshProUGUI countDownText;
        public GameObject roleButtonSection;
        public GameObject lockButton;

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

        // To control coroutine in Update
        private bool countDownStarted = false;

        void Update()
        {
            // Checking UDP gameObject activity to set UI
            if (udp.activeSelf)
            {
                if (UDPChat.instance.connectionFailed)
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

                // Start or stop game depending on gameState

                if (UDPChat.instance.gameState == "start" && !countDownStarted)
                {
                    StartCoroutine(startCountDown());
                    countDownStarted = true;
                }
                else if (UDPChat.instance.gameState == "stop" && countDownStarted)
                {
                    countDownStarted = false;
                }
            }
        }

        // Countdown Text Function
        private static string message = "Game starts in ";
        private static int second;

        private IEnumerator startCountDown()
        {
            for (second = 2; second > 0; second--)
            {
                if (UDPChat.instance.gameState != "stop")
                {
                    instance.countDownText.gameObject.SetActive(true);
                    instance.countDownText.SetText(message + second.ToString());
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    instance.countDownText.SetText("");
                    instance.countDownText.gameObject.SetActive(false);
                    second = 5;
                    break;
                }

                // Load "GameScene" when countdown ends
                if (second == 1)
                {
                    LobbyList.addRolesToPlayerList();
                    DontDestroyOnLoad(udp);
                    SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
                }
            }
        }

        // OnClick Functions
        public void clientButtonFunc()
        {
            ipText = ipInput.text;
            username = usernameInput.text;

            if (checkUserName(username))
            {
                Globals.isServer = false;

                IPAddress ip = IPAddress.Parse(ipText);
                udp.GetComponent<UDPChat>().serverIp = ip;
                udp.SetActive(true);
            }
        }

        public void serverButtonFunc()
        {
            username = usernameInput.text;

            if (checkUserName(username))
            {
                Globals.isServer = true;
                udp.SetActive(true);
                UDPChat.instance.connectionFailed = false;

                startButton.SetActive(true);
            }
        }

        public void exitButtonFunc()
        {
            // Set UDP settings as default
            if (!UDPChat.instance.isServer)
            {
                UDPChat.instance.clientDisconnected();
            }

            UDPChat.instance.connection.Close();
            resetUDP();

            // Set UI elements to show Main Menu
            LobbyList.clearLobbyList();

            // Set everything default state
            UDPChat.instance.readyStatement = "N";
            stopGame();

            roleButtonSection.SetActive(true);
            lockButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Lock";

            lobbyUI.SetActive(false);
            startButton.SetActive(false);
            mainUI.SetActive(true);
        }

        //// Start Button Functions
        public void startButtonFunc()
        {
            if (instance.startButton.transform.GetChild(0).gameObject.GetComponent<Text>().text == "Start")
            {
                // check stateList if there is a unready player

                LobbyList.instance.checkList();

            }
            else
            {
                stopGame();
            }
        }

        //// startGame() and stopGame() are also used in ProtocolHandler
        internal static void startGame()
        {
            if (UDPChat.instance.isServer)
            {
                instance.startButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Stop";

                object[] gameMsg = new object[2] { ProtocolLabels.gameAction, "start" };

                string msg = MessageMaker.makeMessage(gameMsg);

                UDPChat.instance.Send(msg);
            }

            UDPChat.instance.gameState = "start";
        }

        internal static void stopGame()
        {
            if (UDPChat.instance.isServer)
            {
                instance.startButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Start";

                object[] gameMsg = new object[2] { ProtocolLabels.gameAction, "stop" };

                string msg = MessageMaker.makeMessage(gameMsg);

                UDPChat.instance.Send(msg);
            }

            UDPChat.instance.gameState = "stop";
        }

        //// Role Buttons Function
        public void roleButtonFunc(string roleName)
        {
            object[] roleInfo = new object[3]{ ProtocolLabels.roleSelected,
                                               UDPChat.clientNo,
                                               roleName
                                             };

            string msg = MessageMaker.makeMessage(roleInfo);

            UDPChat.instance.roleName = roleName;

            if (UDPChat.instance.isServer)
            {
                // Server must set roleName to lobby list by itself    
                LobbyList.setRolePref(roleName);

                UDPChat.instance.Send(msg);
            }
            else
            {
                UDPChat.instance.connection.Send(msg,
                                            new IPEndPoint(UDPChat.instance.serverIp, Globals.port));
            }
        }

        //// Lock button settings        
        public void lockButtonFunc()
        {
            string buttonName = lockButton.transform.GetChild(0).gameObject.GetComponent<Text>().text;

            //Set lock button's function according to its name
            if (buttonName == "Lock" && LobbyList.instance.imageList[UDPChat.clientNo].color != Color.white)
            {
                lockPref();
            }
            else if (buttonName == "Unlock" && !countDownText.gameObject.activeSelf) // add later 
            {
                unlockPref();
            }
        }

        internal void lockPref()
        {
            UDPChat.instance.readyStatement = "R"; // "R" means "Ready"
            LobbyList.setReadyStatement("R", UDPChat.clientNo);

            object[] readyInfo = new object[3]{ProtocolLabels.clientReady,
                                              UDPChat.clientNo,
                                              "R"};

            string readyMsg = MessageMaker.makeMessage(readyInfo);

            UDPChat.instance.Send(readyMsg);

            roleButtonSection.SetActive(false);

            lockButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Unlock";
        }

        internal void unlockPref()
        {
            const string NOT_READY = "N";

            UDPChat.instance.readyStatement = NOT_READY;
            LobbyList.setReadyStatement(NOT_READY, UDPChat.clientNo);

            object[] unreadyInfo = new object[3]{ProtocolLabels.clientReady,
                                                 UDPChat.clientNo,
                                                 NOT_READY};

            string unreadyMsg = MessageMaker.makeMessage(unreadyInfo);

            UDPChat.instance.Send(unreadyMsg);

            roleButtonSection.SetActive(true);

            lockButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Lock";
        }

        // Common Functions in this script 

        // To check username if it is approative or not
        // @param username cannot start or end with space character   
        private bool checkUserName(string username)
        {
            return
                username.Length != 0 &&
                !username.StartsWith(" ") &&
                !username.EndsWith(" ");
        }

        //// set "udp" a new gameobject with "udpPrefab"
        private void resetUDP()
        {
            Destroy(udp);
            udp = Instantiate(udpPrefab);
            udp.name = "UDP GameObject";
        }
    }
}