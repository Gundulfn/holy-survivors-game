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
        itemId = "a" + itemNo.ToString();

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

        transform.Translate(0, 0, -bulletSpeed * Time.deltaTime);

        Vector3 posDifference = transform.position - prevPos;

        RaycastHit[] hits = Physics.RaycastAll(new Ray(prevPos, posDifference.normalized));

        for(int i = 0; i < hits.Length; i++)
        {
            Debug.Log(hits[i].collider.gameObject.name);

            if(hits[i].collider.gameObject.GetComponent<Demon>())
            {
                Demon demonScript = hits[i].collider.gameObject.GetComponent<Demon>();

                demonScript.setHP(demonScript.getHP() - bulletDamage);

                Debug.Log(demonScript.getHP());

                Destroy(gameObject);        
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
