using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Inventory settings
    private static List<Item> inventoryList = new List<Item>();
    public static Inventory instance;
    private int inventoryListLimit = 6;
    private List<int> invItemIdList = new List<int>(6){0, 1, 2, 3, 4, 5};

    // Other UI settings
    private Transform[] slotTransformArray;
    private List<InventorySlot> inventorySlotList = new List<InventorySlot>();
    private List<GameObject> currentItemIconList = new List<GameObject>();
    
    private int currentItemNo;
    private List<string> slotKeyNames = new List<string>(){"1", "2", "3", "4", "5", "6"};
    
    void Start()
    {
        instance = this;

        slotTransformArray = new Transform[inventoryListLimit];

        for (int i = 0; i < slotTransformArray.Length; i++)
        {
            // "+ 1" shows "SlotImages" GameObject 
            slotTransformArray[i] = transform.GetChild(i + 1);
            inventorySlotList.Add(slotTransformArray[i].GetComponent<InventorySlot>());
        }

        foreach (InventorySlot inventorySlot in inventorySlotList)
        {
            currentItemIconList.Add(inventorySlot.currentItemIcon);
        }

    }
    
    // For start equipments according to player's role
    private string playerRoleName;
    private bool areItemsGiven = false;

    void Update()
    {
        if(!areItemsGiven)
        {
            playerRoleName = GameSceneEventHandler.instance.localPlayer.getRoleName();

            switch(playerRoleName)
            {
                case "lumberjack":
                    addItem(ItemId.woodenAxe);
                    break;
                case "musketeer":
                    addItem(ItemId.musket);
                    addItem(ItemId.musketBall, 10);
                    break;
                case "pirate":
                    addItem(ItemId.cutlass);
                    addItem(ItemId.pistov);
                    addItem(ItemId.pistovBall, 15);
                    break;
                case "royalGuard":
                    addItem(ItemId.spear);
                    break;
                default:
                    break;    
            }

            areItemsGiven = true;
        }

        // set @param currentItemNo by mouseScrollWheel input and actions
        float mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (mouseScrollInput != 0)
        {
            if (mouseScrollInput > 0f) // forward
            {
                currentItemNo++;
            }
            else if (mouseScrollInput < 0f) // backwards
            {
                currentItemNo--;
            }

            currentItemNo = Mathf.Clamp(currentItemNo, 0, 5);
            setCurrentItemIconActive();
        }
        else if(slotKeyNames.Contains(Input.inputString))
        {
            currentItemNo = System.Int32.Parse(Input.inputString) - 1;            
            setCurrentItemIconActive();
        }

        handleInventoryItem();
    }

    public bool canTakeItemWithId(string itemId)
    {    
        bool hasAvailableSlot = inventoryList.ToArray().Length < inventoryListLimit;
        Item sameItem = inventoryList.Find(item =>
        {
            return item.getItemId() == itemId && item.getStackCount() < item.getStackLimit();
        });

        return sameItem != null || hasAvailableSlot;

    }

    public void addItem(string itemId, int count = 0)
    {
        Item sameItem = inventoryList.Find(item => item.getItemId() == itemId && item.getStackCount() < item.getStackLimit());

        if (sameItem == null)
        {   
            Item newItem = new Item(itemId);

            if(count == 0)
            {
                newItem.setStackCount(newItem.getStackCount() + 1);
            }
            else
            {
                newItem.setStackCount(newItem.getStackCount() + count);
            }
            
            inventoryList.Add(newItem);
            newItem.setInvItemId(invItemIdList[0]);
            invItemIdList.Remove(invItemIdList[0]);

            ItemPrefab.instance.addObj(newItem, newItem.getInvItemId());

            setItemIconInvItemId(newItem);
            setItemIconImgUI(newItem);
        }
        else
        {
            // Stack item
            if(count == 0)
            {
                sameItem.setStackCount(sameItem.getStackCount() + 1);
            }
            else
            {
                sameItem.setStackCount(sameItem.getStackCount() + 1);
            }            
        }
    }

    public Item getItemWithInvItemId(int invItemNo)
    {
        return inventoryList.Find(item => item.getInvItemId() == invItemNo);
    }

    public Item findItemWithItemId(string itemId)
    {        
        return inventoryList.Find(item => item.getItemId() == itemId);
    }

    // InventoryUI Functions
    private void setItemIconInvItemId(Item item)
    {
        int invItemId = item.getInvItemId();

        foreach(InventorySlot invSlot in inventorySlotList)
        {
            if(invSlot.itemIcon.getInvItemId() < 0)
            {
                invSlot.itemIcon.setInvItemId(invItemId);
                break;
            }
        }
    }

    private void setItemIconImgUI(Item item)
    {
        int invItemId = item.getInvItemId();

        foreach(InventorySlot invSlot in inventorySlotList)
        {
            if(invSlot.itemIcon.getInvItemId() == invItemId)
            {
                invSlot.itemIcon.setItemIconImg(item);
                break;
            }
        }
    }

    // CurrentItem Functions
    private void setCurrentItemIconActive()
    {
        foreach (GameObject currentItemIcon in currentItemIconList)
        {
            if (currentItemIcon != currentItemIconList[currentItemNo])
            {
                currentItemIcon.SetActive(false);
            }
            else
            {
                currentItemIcon.SetActive(true);
            }
        }
    }

    private void handleInventoryItem()
    {
        ItemIcon itemIcon = inventorySlotList[currentItemNo].itemIcon;

        if (itemIcon.getInvItemId() >= 0 )
        {
            int invItemNo = itemIcon.getInvItemId();
            Item currentItem = inventoryList.Find(item => item.getInvItemId() == itemIcon.getInvItemId());

            ItemPrefab.instance.handleInventoryItemEvent(invItemNo);

            if(Input.GetKeyDown(KeyCode.G))
            {
                dropItem(itemIcon, invItemNo, currentItem);
            }
        }
        else
        {
            ItemPrefab.instance.handleInventoryItemEvent();
        }
    }

    public void removeCurrentItem()
    {
        ItemIcon itemIcon = inventorySlotList[currentItemNo].itemIcon;
        int invItemId = itemIcon.getInvItemId();
        Item currentItem = inventoryList.Find(item => item.getInvItemId() == invItemId);

        inventoryList.Remove(currentItem);
        invItemIdList.Add(invItemId);
        ItemPrefab.instance.destroyObj(invItemId);
        itemIcon.resetItemIcon();
    }

    // Remove specific item
    public void removeItem(Item item)
    {
        int invItemId = item.getInvItemId();
        InventorySlot invSlot = inventorySlotList.Find(slot => slot.itemIcon.getInvItemId() == invItemId);
        ItemIcon itemIcon = invSlot.itemIcon;

        inventoryList.Remove(item);
        invItemIdList.Add(invItemId);
        ItemPrefab.instance.destroyObj(invItemId);
        itemIcon.resetItemIcon();
    }

    // DropItem Funcitons
    // For param@ ItemDragHandler Actions
    public void dropItem(ItemIcon itemIcon)
    {
        int invItemId = itemIcon.getInvItemId();
        Item droppedItem = inventoryList.Find(item => item.getInvItemId() == invItemId);
        
        inventoryList.Remove(droppedItem);
        invItemIdList.Add(invItemId);
        ItemPrefab.instance.dropObj(droppedItem.getItemType(), invItemId);
        itemIcon.resetItemIcon();
    }
    
    // Only use in Inventory
    private void dropItem(ItemIcon itemIcon, int itemIndex, Item currentItem)
    {
        inventoryList.Remove(currentItem);
        invItemIdList.Add(currentItem.getInvItemId());
        ItemPrefab.instance.dropObj(currentItem.getItemType(), itemIndex);
        itemIcon.resetItemIcon();
    }
}