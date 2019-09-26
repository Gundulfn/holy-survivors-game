using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public int itemNo;
    public bool isLoot;

    private string itemId;
    private int invItemNo = -1;
    private Item food;
    private Player localPlayer;
    private CooldownScript cooldownScript;
    
    void Awake()
    {
        itemId = ItemType.food + itemNo.ToString();
        food = new Item(itemId);
        
        gameObject.name = food.getItemName();
        cooldownScript = new CooldownScript();
        localPlayer = GameObject.Find("GameEditor").GetComponent<GameSceneEventHandler>().localPlayer;       
    }

    void Update()
    {
        if(!isLoot)
        {
            MouseFunctions.mouseClickActions(itemId, gameObject);   

            if(invItemNo != -1)
            {
                food = Inventory.instance.getItemWithInvItemId(invItemNo);
            }         
        }
    }

    public void setInvItemNo(int value)
    {
        invItemNo = value;
    }

    public void setFoodStackCount(int value)
    {
        food.setStackCount(value);
    }

    private void checkStackCount()
    {
        if(food.getStackCount() == 1)
        {
            Inventory.instance.removeCurrentItem();
        }
        else
        {
            food.setStackCount(food.getStackCount() - 1);
        }
    }

    private int breadHPRegen = 15;

    internal void breadFunc()
    {
        int hp = localPlayer.getHP();
        int defaultHP = localPlayer.getDefaultHP();

        if(hp < defaultHP)
        {
            if(hp + breadHPRegen >= defaultHP)
            {
                localPlayer.setHP(defaultHP);
            }
            else
            {
                localPlayer.setHP(hp + breadHPRegen);
            }

            checkStackCount();
        }
    }

    private int waterBottleSPRegen = 15;

    internal void waterBottleFunc()
    {
        float sp = localPlayer.getStamina();
        float defaultSP = localPlayer.getDefaultSP();

        if(sp < defaultSP)
        {
            if(sp + waterBottleSPRegen >= defaultSP)
            {
                localPlayer.setStamina(defaultSP);
            }
            else
            {
                localPlayer.setStamina(sp + waterBottleSPRegen);
            }
        }

        checkStackCount();
    }
}
