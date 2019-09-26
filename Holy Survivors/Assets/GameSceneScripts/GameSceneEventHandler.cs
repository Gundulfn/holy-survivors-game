using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSceneEventHandler : MonoBehaviour
{
    public static GameSceneEventHandler instance;

    public GameObject musketeerPrefab;
    public GameObject royalGuardPrefab;
    public GameObject piratePrefab;
    public GameObject lumberjackPrefab;

    //Player Data
    public PlayerStatus playerStatus;
    public Player localPlayer;

    // localPlayer's CameraRay Component Settings
    public HitItemInfoUI hitItemInfo;

    //public static TextMeshProUGUI debug;
    private static List<GameObject> playerObjList = new List<GameObject>();

    void Start()
    {
        instance = this;
        // debug = GameObject.Find("yo").GetComponent<TextMeshProUGUI>();
        // Spawn all players' bodies by their rolenames 
        // foreach(string player in HD.UDPChat.instance.playerList)
        // {
        //     if(player != "" && player.Contains(";"))
        //     {
        //         spawnPlayerGameObj(player);
        //         setLobbyList(player);
        //     }
        // }
        
        spawnPlayerGameObj("boss;pirate");
    }

    ////////////////////////////////////////////////////////////////////
    // Beginning Functions
    
    // Instantiate playerGameObject according to data in 'player' string
    private void spawnPlayerGameObj(string player)
    {
        //int clientNo = HD.UDPChat.instance.playerList.IndexOf(player);
        int clientNo = 0;
        string[] sections = player.Split(';');

        switch(sections[1])
        {
            case "musketeer":
                GameObject musketeerGameObj = Instantiate(musketeerPrefab);
                musketeerGameObj.name = sections[0] + "|" + clientNo;
                editPlayerGameObj(musketeerGameObj, sections[0], sections[1]);
                break;
            
            case "lumberjack":
                GameObject lumberjackGameObj = Instantiate(lumberjackPrefab);
                lumberjackGameObj.name = sections[0] + "|" + clientNo;
                editPlayerGameObj(lumberjackGameObj, sections[0], sections[1]);
                break;
            
            case "pirate":
                GameObject pirateGameObj = Instantiate(piratePrefab);
                pirateGameObj.name = sections[0] + "|" + clientNo;
                editPlayerGameObj(pirateGameObj, sections[0], sections[1]);
                break;
            
            case "royalGuard":
                GameObject royalGuardGameObj = Instantiate(royalGuardPrefab);
                royalGuardGameObj.name = sections[0] + "|" + clientNo;
                editPlayerGameObj(royalGuardGameObj, sections[0], sections[1]);
                break;            
        }
    }

    // Check playerGameObject if it's local player or another one
    private void editPlayerGameObj(GameObject cloneGameObj, string playerName, string roleName)
    {
        //string player = HD.UDPChat.instance.username + "|" + HD.UDPChat.clientNo;
        string player = "boss" + "|" + "0";
        
        if(cloneGameObj.name == player)
        {
            // It's local player gameobject
            localPlayer = cloneGameObj.GetComponent<Player>();
            
            playerStatus.player = cloneGameObj.GetComponent<Player>();
            cloneGameObj.GetComponent<Player>().setRoleName(roleName);

            CameraRay camRay = cloneGameObj.transform.GetChild(0).gameObject.AddComponent<CameraRay>();
            camRay.hitItemInfo = hitItemInfo;
            camRay.mask = 2;
        }   
        else
        {
            // It's another player's gameobject, delete playerController and change cam display
            PlayerController playerCont = cloneGameObj.GetComponent<PlayerController>();
            Camera camera = cloneGameObj.transform.GetChild(0).gameObject.GetComponent<Camera>();

            camera.targetDisplay = 1;

            Destroy(playerCont);
        }

        TextMeshPro playerNameText = cloneGameObj.GetComponent<Player>().nameText;

        playerNameText.SetText(playerName);

        // Add object to playerObjList
        playerObjList.Add(cloneGameObj);

        // spawn playerObjects at specific positions depending on their index in playerObjList
        //int index = playerObjList.IndexOf(cloneGameObj);
        int index = 0;
        
        switch(index)
        {
            case 0:
                cloneGameObj.transform.position = new Vector3(0, 1, 0);
                break;

            case 1:
                cloneGameObj.transform.position = new Vector3(2, 1, 1);
                break;

            case 2:
                cloneGameObj.transform.position = new Vector3(-2, 1, 3);
                break;

            case 3:
                cloneGameObj.transform.position = new Vector3(4, 0, 2);
                break;

            default:
                break;                
        }
    }
    
    // To set playerNames to lobby
    private void setLobbyList(string player)
    {
        string[] data = player.Split(';');
        // int playerSectionNo = HD.UDPChat.instance.playerList.IndexOf(player);
        int playerSectionNo = 0;
        HD.LobbyList.setPlayerName(data[0], playerSectionNo);
        HD.LobbyList.setRolePref(data[1], playerSectionNo);
    }
    /////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////
    // Protocol Action Functions

    public static void movePlayerObj(int playerNo, float[] coordinates)
    {
        Vector3 pos = new Vector3(coordinates[0],
                                  coordinates[1], 
                                  coordinates[2]);

        playerObjList[playerNo].transform.position = pos;
    }

    public static void rotatePlayerObj(int playerNo, float rotX, float rotY)
    {       
        Transform playerTransform = playerObjList[playerNo].transform;

        playerTransform.eulerAngles = new Vector3(0, rotY, 0);
        
        // playerObj's Camera Rotation
		playerTransform.GetChild(0).eulerAngles = new Vector3(rotX, playerTransform.eulerAngles.y, 0);
    }
    /////////////////////////////////////////////////////////////////////
}