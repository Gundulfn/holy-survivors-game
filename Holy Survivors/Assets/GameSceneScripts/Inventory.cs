using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static List<Item> inventoryList = new List<Item>();
    public static Inventory instance;
    private int inventoryListLimit = 6;

    void Start()
    {
        instance = this;
    }

    public bool canTakeItemWithId(string itemId)
    {
        bool hasAvailableInventorySlot = inventoryList.ToArray().Length <= inventoryListLimit;
        Item sameItem = inventoryList.Find(item => item.getItemId() == itemId && item.stackCount < item.getStackLimit());

        return sameItem != null || hasAvailableInventorySlot;
    }

    public void addItem(string itemId)
    {
        Item sameItem = inventoryList.Find(item => item.getItemId() == itemId && item.stackCount < item.getStackLimit());

        if (sameItem == null)
        {
            // Add item to a new inventory slot 
            Item item = new Item(itemId);
            item.stackCount += 1;

            inventoryList.Add(item);
        }
        else
        {
            // Stack item
            sameItem.stackCount += 1;
        }
    }
}