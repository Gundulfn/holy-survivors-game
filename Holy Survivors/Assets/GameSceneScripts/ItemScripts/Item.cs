using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private int stackCount;
    private string itemName;
    private string itemId;
    private string itemType;
    private int stackLimit;

    private int invItemId;

    public Item(string itemId)
    {
        setItemId(itemId);
        setItemName();
        setItemType();
        setStackLimit();
        setStackCount();
    }

    private void setItemId(string value)
    {
        itemId = value;
    }

    private void setStackLimit()
    {
        string itemType = itemId.Substring(0, 1);

        if (itemType == ItemType.weapon)
        {
            stackLimit = 1;
        }
        else
        {
            switch (itemId)
            {
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
    }

    private void setItemName()
    {
        switch (itemId)
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
                itemName = "Musket Ball";
                break;
            case ItemId.pistovBall:
                itemName = "Pistov Ball";
                break;
            default:
                break;
        }
    }

    private void setItemType()
    {
        itemType = itemId.Substring(0, 1);
    }

    public void setStackCount(int value = 0)
    {
        stackCount = value;
    }

    public void setInvItemId(int value)
    {
        invItemId = value;
    }

    public string getItemId()
    {
        return itemId;
    }

    public int getStackLimit()
    {
        return stackLimit;
    }

    public string getItemName()
    {
        return itemName;
    }

    public string getItemType()
    {
        return itemType;
    }

    public int getStackCount()
    {
        return stackCount;
    }

    public int getInvItemId()
    {
        return invItemId;
    }
}