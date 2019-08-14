using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Character
{
    public int x;
    // Start is called before the first frame update
    void Start()
    {
        setHP(100);
    }

    // Update is called once per frame
    void Update()
    {
        x = getHP();
    }
}
