using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int itemNo;
    public bool isLoot;

    private int invItemNo = -1;
    private string itemId;
    private Item ammo;

    void Start()
    {
        if(isLoot && GetComponent<Bullet>())
        {
            gameObject.AddComponent<SphereCollider>();
            GetComponent<Bullet>().enabled = false;
        }

        itemId = ItemType.ammo + itemNo.ToString();

        ammo = new Item(itemId);
        gameObject.name = ammo.getItemName();
    }

    void Update()
    {
        if(!isLoot)
        {
            if(invItemNo != -1)
            {
                ammo = Inventory.instance.getItemWithInvItemId(invItemNo);
            }             
        }
    }

    public void setInvItemNo(int value)
    {
        invItemNo = value;
    }
}
