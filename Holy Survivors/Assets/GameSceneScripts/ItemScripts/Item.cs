using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int stackCount;

    private string itemName;
    private string itemId;
    private int stackLimit;

    public Item(){  }

    public Item(string itemId)
    {
        setItemId(itemId);
    }

    // When itemId is set, it will set "stackLimit" and "itemName"
    private void setItemId(string value)
    {
        itemId = value;
        
        setItemName();
        setStackLimit();
    }

    private void setStackLimit()
    {
        string itemType = itemId.Substring(0, 1);

        switch(itemType)
        {
            case "w":
                stackLimit = 1;
                break;
            case ItemId.bread:
                stackLimit = 10;
                break;
            case ItemId.waterBottle:
                stackLimit = 8;
                break;
            case ItemId.gunpowder:
                stackLimit = 50;
                break;    
            case ItemId.musketBall:
                stackLimit = 30;
                break;
            case ItemId.pistovBall:
                stackLimit = 50;
                break;    
            default:
                break;
        }
    }
    
    private void setItemName()
    {
        switch(itemId)
        {
            case ItemId.musket:
                itemName = "Musket";
                break;
            case ItemId.pistov:
                itemName = "Pistov";
                break;
            case ItemId.cutlass:
                itemName = "Cutlass";
                break;
            case ItemId.woodenAxe:
                itemName = "Wooden Axe";
                break;
            case ItemId.spear:
                itemName = "Spear";
                break;
            case ItemId.bread:
                itemName = "Bread";
                break;
            case ItemId.waterBottle:
                itemName = "Water Bottle";
                break;
            case ItemId.gunpowder:
                itemName = "Gunpowder";
                break;    
            case ItemId.musketBall:
                itemName = "Rifle Ball";
                break;
            case ItemId.pistovBall:
                itemName = "Pistov Ball";
                break;
            default:
                break;
        }
    }

    public string getItemId()
    {
        return itemId;
    }

    public string getItemName()
    {
        return itemName;
    }

    public int getStackLimit()
    {
        return stackLimit;
    }
}