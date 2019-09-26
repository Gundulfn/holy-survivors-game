using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefab: MonoBehaviour
{
    public static ItemPrefab instance;
    
    private const string weaponPrefabFilePath = "Prefabs/Weapons/";
    private const string foodPrefabFilePath = "Prefabs/Foods/";
    private const string ammoPrefabFilePath = "Prefabs/Ammos/"; 

    private Dictionary<int, GameObject> invObjDict = new Dictionary<int, GameObject>();
    public int a;

    void Start()
    {
        instance = this;    
    }

    public void handleInventoryItemEvent(int itemNo = -1)
    {
        if(itemNo != -1)
        {
            a = itemNo;
            setObjActive(itemNo);
        }
        else
        {
            foreach(GameObject inventoryObj in invObjDict.Values.ToList())
            {
                if(inventoryObj != null)
                {
                    inventoryObj.SetActive(false);
                }
            }
        }
    }

    public void dropObj(string itemType, int invItemId)
    {
        GameObject droppedObj = invObjDict[invItemId];
        
        invObjDict.Remove(invItemId);        

        droppedObj.transform.parent = null;
        droppedObj.AddComponent<Rigidbody>();
        
        switch(itemType)
        {
            case ItemType.weapon:
                droppedObj.GetComponent<Weapon>().isLoot = true;
                break;
            
            case ItemType.food:
                droppedObj.GetComponent<Food>().isLoot = true;
                break;
            
            case ItemType.ammo:
                droppedObj.GetComponent<Ammo>().isLoot = true;
                droppedObj.AddComponent<SphereCollider>();
                break;

            default:
                break;
        }

        droppedObj.SetActive(true);
    }

    public void destroyObj(int invItemId)
    {
        GameObject consumedObj = invObjDict[invItemId];
        Destroy(consumedObj);
    }

    private bool isLootWeaponLoaded;

    public void addObj(Item item, int invItemId)
    {
        GameObject objPrefab = (GameObject) Resources.Load( getPrefabFilePath(item) );
        Vector3 objEulerAngles = objPrefab.transform.eulerAngles;

        GameObject obj = Instantiate(objPrefab, transform.position, transform.rotation);
        obj.transform.parent = transform;
        obj.transform.localEulerAngles = objEulerAngles;

        invObjDict.Add(invItemId, obj);
        
        switch(item.getItemType())
        {
            case ItemType.weapon:
                obj.GetComponent<Weapon>().canUse = isLootWeaponLoaded;
                break;

            case ItemType.food:
                obj.GetComponent<Food>().setInvItemNo(invItemId);
                break;    

            case ItemType.ammo:
                obj.GetComponent<Ammo>().setInvItemNo(invItemId);
                Destroy(obj.GetComponent<Bullet>());
                break;
        }
    }

    public void setIsLootWeaponLoaded(bool value)
    {
        isLootWeaponLoaded = value;
    }

    private void setObjActive(int invItemId)
    {
        foreach(GameObject inventoryObj in invObjDict.Values.ToList())
        {
            if(inventoryObj != null)
            {
                if(inventoryObj == invObjDict[invItemId])
                {
                    inventoryObj.SetActive(true);
                }
                else
                {
                    inventoryObj.SetActive(false);
                }   
            }
        }
    }

    private string getPrefabFilePath(Item item)
    {
        string prefabFilePath = "";

        switch(item.getItemType())
        {
            case ItemType.weapon:
                prefabFilePath += weaponPrefabFilePath + item.getItemName();
                break;

            case ItemType.food:
                prefabFilePath += foodPrefabFilePath + item.getItemName();
                break;    

            case ItemType.ammo:
                prefabFilePath += ammoPrefabFilePath + item.getItemName();
                break;
            default:
                break;    
        }

        return prefabFilePath;
    }
}
