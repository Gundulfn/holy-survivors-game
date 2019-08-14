using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int itemNo;
    private string itemId;
    private Item ammo;

    void Start()
    {
        itemId = "a" + itemNo.ToString();

        ammo = new Item(itemId);
        gameObject.name = ammo.getItemName();
    }
}
