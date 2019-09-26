using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private DemonAI demonAI;

    void Awake()
    {
        demonAI = transform.parent.GetComponent<DemonAI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Player>())
        {
            demonAI.enabled = true;
            demonAI.targetPlayer = other.gameObject.GetComponent<Player>();
        }
    }
}
