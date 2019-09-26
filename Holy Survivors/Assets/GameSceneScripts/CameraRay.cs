using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraRay : MonoBehaviour {

	public LayerMask mask;
	
	internal HitItemInfoUI hitItemInfo;
	private TextMeshProUGUI hitItemText;
	
	private GameObject hitItemInfoObj;
    private GameObject actionTakeTextObj;

	void Start ()
    {
		hitItemText = hitItemInfo.hitItemText;

		hitItemInfoObj = hitItemInfo.gameObject;
        actionTakeTextObj = hitItemInfo.actionTakeTextObj;
	}

	void Update () 
    {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo, 10, mask, QueryTriggerInteraction.Ignore)) 
		{

			GameObject detectedGameObj = hitInfo.collider.gameObject;

			hitItemText.SetText(detectedGameObj.name);

			if(detectedGameObj.GetComponent<Weapon>())
            {	
				if(detectedGameObj.GetComponent<Weapon>().isLoot)
				{
					hitItemInfoObj.SetActive(true);
					actionTakeTextObj.SetActive(true);

					string itemId = ItemType.weapon + detectedGameObj.GetComponent<Weapon>().itemNo.ToString();
					
					takeItem(detectedGameObj, itemId);
				}	
			}
			else if(detectedGameObj.GetComponent<Food>())
            {			
				hitItemInfoObj.SetActive(true);
				actionTakeTextObj.SetActive(true);

				string itemId = ItemType.food + detectedGameObj.GetComponent<Food>().itemNo.ToString();
				
				takeItem(detectedGameObj, itemId);
			}
			else if(detectedGameObj.GetComponent<Ammo>())
			{
				hitItemInfoObj.SetActive(true);
				actionTakeTextObj.SetActive(true);

				string itemId = ItemType.ammo + detectedGameObj.GetComponent<Ammo>().itemNo.ToString();
				
				takeItem(detectedGameObj, itemId);
			}
            else
            {
				hitItemInfoObj.SetActive(false);
			}
			
		} 
		else 
		{
			hitItemInfoObj.SetActive(false);
			actionTakeTextObj.SetActive(false);

			hitItemText.SetText("");
			Debug.DrawLine (ray.origin, ray.origin + ray.direction * 10, Color.green);
		}
		
	}

	private void takeItem(GameObject itemObj, string itemId)
	{
		if(Input.GetKeyDown(KeyCode.E) && Inventory.instance.canTakeItemWithId(itemId))
		{
			if(itemObj.GetComponent<Weapon>())
			{
				Debug.Log("CameraRay: " + itemObj.GetComponent<Weapon>().canUse);
				ItemPrefab.instance.setIsLootWeaponLoaded(itemObj.GetComponent<Weapon>().canUse);
			}

			Inventory.instance.addItem(itemId);
			Destroy(itemObj);
		}
	}
}