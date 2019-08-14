using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int itemNo;
    
    private string itemId;
    private Item food;
    private Player localPlayer;
    private CooldownScript cooldownScript;
    
    void Start()
    {
        itemId = "f" + itemNo.ToString();

        if(itemId.Length == 2)
        {
            food = new Item(itemId);
        }

        localPlayer = GameObject.Find("GameEditor").GetComponent<GameSceneEventHandler>().localPlayer;
        cooldownScript = new CooldownScript();
        gameObject.name = food.getItemName();
    }

    void Update()
    {
        MouseFunctions.mouseClickActions(itemId, gameObject);
    }

    internal void breadFunc()
    {

    }

    internal void waterBottleFunc()
    {

    }
}
