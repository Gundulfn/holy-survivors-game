using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawn : MonoBehaviour
{
    public GameObject demonPrefab;
    private string spawnPos;

    void Awake()
    {
        spawnPos = transform.position.ToString();
        GameObject demonObj = Instantiate(demonPrefab, transform.position, transform.rotation);
        demonObj.GetComponent<Demon>().setDemonId(spawnPos); 
    }
}
