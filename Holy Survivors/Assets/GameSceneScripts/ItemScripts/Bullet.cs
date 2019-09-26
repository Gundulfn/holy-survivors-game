using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private string itemId;
    private int bulletDamage;
    private int bulletSpeed = 200;
    private Vector3 prevPos;

    void Start()
    {
        int itemNo = GetComponent<Ammo>().itemNo;
        itemId = ItemType.ammo + itemNo.ToString();

        switch(itemId)
        {
            case ItemId.musketBall:
                bulletDamage = 50;
                break;

            case ItemId.pistovBall:
                bulletDamage = 30;
                break;

            default:
                break;    
        }
    }

    void Update()
    {
        prevPos = transform.position;

        if(itemId == ItemId.musketBall)
        {
            transform.Translate(0, 0, bulletSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(0, 0, bulletSpeed * Time.deltaTime);
        }

        Vector3 posDifference = transform.position - prevPos;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, posDifference.normalized));

        for(int i = 0; i < hits.Length; i++)
        {   

            if(hits[i].collider.gameObject.GetComponent<Demon>())
            {
                Demon demon = hits[i].collider.gameObject.GetComponent<Demon>();

                demon.setHP(demon.getHP() - bulletDamage);
                Destroy(gameObject);  
                break;      
            }
            else if(hits[i].collider.gameObject.tag == "environment")
            {
                Debug.Log(hits[i].collider.gameObject.name);
                Destroy(gameObject);
            }
        }
    }
}
