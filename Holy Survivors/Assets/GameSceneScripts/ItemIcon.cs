using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemIcon : MonoBehaviour
{
    public int invItemId = -1;
    private Image itemIconImg;
    private Sprite defaultImgSprite;
    private TextMeshProUGUI stackCountText;

    private ItemDragHandler itemDragHandler;

    void Awake()
    {
        itemDragHandler = GetComponent<ItemDragHandler>();
        itemIconImg = gameObject.GetComponent<Image>();
        stackCountText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        defaultImgSprite = itemIconImg.sprite;
    }

    void Update()
    {
        if(invItemId != -1)
        {
            Item invItem = Inventory.instance.getItemWithInvItemId(invItemId);
            
            int currentStackCount = invItem.getStackCount();

            stackCountText.SetText(currentStackCount.ToString());

            if(UIHandler.instance.getActiveUINo() == 1)
            {
                itemDragHandler.canCarryItemIcon = true;
            }
        }
        else
        {
            itemDragHandler.canCarryItemIcon = false;
        }
    }
    
    public void setInvItemId(int value)
    {
        invItemId = value;
    }

    public int getInvItemId()
    {
        return invItemId;
    }

    public void setItemIconImg(Item item)
    {
        itemIconImg.sprite = getItemIcon(item);
    }

    public void resetItemIcon()
    {
        invItemId = -1;
        stackCountText.SetText("");
        itemIconImg.sprite = defaultImgSprite;
    }

    private Sprite getItemIcon(Item item)
    {
        string itemName = item.getItemName();
        string itemType = item.getItemType();

        string iconFilePath = "Icons/";

        switch (itemType)
        {
            case ItemType.weapon:
                iconFilePath += "WeaponIcons/" + itemName + "Icon";
                break;

            case ItemType.food:
                iconFilePath += "FoodIcons/" + itemName + "Icon";
                break;

            case ItemType.ammo:
                iconFilePath += "AmmoIcons/" + itemName + "Icon";
                break;

            default:
                break;
        }

        return Resources.Load<Sprite>(iconFilePath);
    }
}