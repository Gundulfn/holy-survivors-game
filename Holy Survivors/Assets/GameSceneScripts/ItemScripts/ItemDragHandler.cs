using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{    
    // Component shortnames
    // _T => transform
    // _IS => InventorySlot
    public bool canCarryItemIcon = false;
    private static ItemIcon carriedItemIcon;
    private static Transform carriedItemIcon_T;
    private static InventorySlot carriedItemIcon_IS;
    private static Vector3 prevPos;

    private static List<RectTransform> iconPanelList = new List<RectTransform>();
    private static RectTransform hitIconPanel;

    void Awake()
    {
        iconPanelList.Add(GetComponent<RectTransform>());
    }
    
    public void OnBeingDrag(PointerEventData eventData)
    {
        if(canCarryItemIcon)
        {
            prevPos = transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(canCarryItemIcon)
        {
            transform.position = Input.mousePosition;

            carriedItemIcon = GetComponent<ItemIcon>();
            carriedItemIcon_T = transform;
            carriedItemIcon_IS = transform.parent.GetComponent<InventorySlot>();    
        }       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(canCarryItemIcon)
        {
            getHitIconPanel();
            RectTransform invPanel = transform.parent.parent.GetComponent<RectTransform>();

            if(hitIconPanel != null)
            {
                changeItemIconsDatas(hitIconPanel);
            }
            else if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
            {
                Inventory.instance.dropItem(carriedItemIcon);
                transform.localPosition = Vector3.zero;
                resetCarriedItemIconSettings();
            }
            else
            {
                carriedItemIcon_T.localPosition = Vector3.zero;
                resetCarriedItemIconSettings();
            }
        }
    }

    private void getHitIconPanel()
    {
        foreach(RectTransform iconPanel in iconPanelList)
        {
            if(RectTransformUtility.RectangleContainsScreenPoint(iconPanel, Input.mousePosition)
                && iconPanel != carriedItemIcon_T as RectTransform)
            {
                hitIconPanel = iconPanel;
            }
        }
    }

    private void changeItemIconsDatas(RectTransform hitIconPanel)
    {
        ItemIcon hitItemIcon = hitIconPanel.GetComponent<ItemIcon>();
        Transform hitItemIcon_T = hitIconPanel.GetComponent<Transform>();
        InventorySlot hitItemIcon_IS = hitItemIcon_T.parent.GetComponent<InventorySlot>();

        hitItemIcon_IS.itemIcon = carriedItemIcon;
        carriedItemIcon_IS.itemIcon = hitItemIcon;

        // SetParent Method will be used
        carriedItemIcon_T.SetParent(hitItemIcon_IS.GetComponent<Transform>(), false);
        hitItemIcon_T.SetParent(carriedItemIcon_IS.GetComponent<Transform>(), false);

        carriedItemIcon_T.localPosition = Vector3.zero;
        hitItemIcon_T.localPosition = Vector3.zero;

        carriedItemIcon_T.SetSiblingIndex(0);
        hitItemIcon_T.SetSiblingIndex(0);

        resetCarriedItemIconSettings();
        hitIconPanel = null;
    }

    private void resetCarriedItemIconSettings()
    {
        carriedItemIcon = null;
        carriedItemIcon_T = null;
        carriedItemIcon_IS = null; 
    }
}