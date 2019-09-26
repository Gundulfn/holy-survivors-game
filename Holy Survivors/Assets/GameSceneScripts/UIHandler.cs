using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;
    public GameObject charPanelUI;
    public GameObject menuUI;
    
    private PlayerController playerCont;
    
    private int activeUINo;
    private int menuNo = 0;
    private int charPanelNo = 1;
    private int inGameScreenNo = 2;
    
    void Start()
    {
        instance = this;
        activeUINo = inGameScreenNo;
        playerCont = GameSceneEventHandler.instance.localPlayer.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(activeUINo == menuNo)
            {
                activeUINo = inGameScreenNo;
            }
            else
            {
                activeUINo = menuNo;
            }
        }
        else if(Input.GetKeyDown(KeyCode.C) && activeUINo != 0)
        {
            if(activeUINo == charPanelNo)
            {
                activeUINo = inGameScreenNo;
            }
            else
            {
                activeUINo = charPanelNo;
            }
        }

        switch(activeUINo)
        {
            case 0:
                charPanelUI.SetActive(false);
                menuUI.SetActive(true);
                playerCont.enableControl = false;
                break;

            case 1:
                charPanelUI.SetActive(true);
                menuUI.SetActive(false);
                playerCont.enableControl = false;
                break;

            case 2:
                charPanelUI.SetActive(false);
                menuUI.SetActive(false);
                playerCont.enableControl = true;
                break;        
        }
    }

    public int getActiveUINo()
    {
        return activeUINo;
    }
}
