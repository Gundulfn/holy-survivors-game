using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAttack : MonoBehaviour
{
    private int demonAttackDamage;

    void Start()
    {
        demonAttackDamage = transform.parent.GetComponent<Demon>().getDemonAttackDamage();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>())
        {
            Player hitPlayer = other.gameObject.GetComponent<Player>();
            hitPlayer.setHP( hitPlayer.getHP() - demonAttackDamage);
        }
    }
}
