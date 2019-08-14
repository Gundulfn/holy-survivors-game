using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFunctions : MonoBehaviour
{
    public static GameObject itemObj;

    public static void mouseClickActions(string itemId, GameObject funcItemObj)
    {
        itemObj = funcItemObj;

        if(Input.GetMouseButtonDown(0))
        {
            leftClickFuncs(itemId);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            rightClickFuncs(itemId);
        }

    }

    private static void leftClickFuncs(string itemId)
    {
        switch(itemId)
        {
            case ItemId.musket:
                itemObj.GetComponent<Weapon>().musketLeftFunc();
                break;
            case ItemId.pistov:
                itemObj.GetComponent<Weapon>().pistovLeftFunc();
                break;
            case ItemId.cutlass:
                itemObj.GetComponent<Weapon>().cutlassLeftFunc();
                break;    
            case ItemId.woodenAxe:
                itemObj.GetComponent<Weapon>().woodenAxeLeftFunc();
                break;
            case ItemId.spear:
                itemObj.GetComponent<Weapon>().spearLeftFunc();
                break;            
            default:
                break;
        }
    }

    private static void rightClickFuncs(string itemId)
    {
        switch(itemId)
        {
            // Weapon Items
            case ItemId.musket:
                itemObj.GetComponent<Weapon>().musketRightFunc();
                break;
            case ItemId.pistov:
                itemObj.GetComponent<Weapon>().pistovRightFunc();
                break;
            case ItemId.cutlass:
                itemObj.GetComponent<Weapon>().cutlassRightFunc();
                break;    
            case ItemId.woodenAxe:
                itemObj.GetComponent<Weapon>().woodenAxeRightFunc();
                break;
            case ItemId.spear:
                itemObj.GetComponent<Weapon>().spearRightFunc();
                break;

            // Food Items
            case ItemId.bread:
                itemObj.GetComponent<Food>().breadFunc();
                break;
            case ItemId.waterBottle:
                itemObj.GetComponent<Food>().waterBottleFunc();
                break;    
            default:
                break;
        }
    }
}