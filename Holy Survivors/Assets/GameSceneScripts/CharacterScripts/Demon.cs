using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Character
{
    public int demonNo;
    private string demonId;
    private float demonSpeed;
    private int demonAttackDamage;
    
    void Awake()
    {
        switch(demonNo)
        {
            case 0:
                setHP(10);
                demonSpeed = 6;
                demonAttackDamage = 1;
                break;

            case 1:
                setHP(50);
                demonSpeed = 3;
                demonAttackDamage = 10;
                break;       
        }
    }

    void Update()
    {
        if(getHP() <= 0)
        {
            object[] msgParts = new object[2]{HD.ProtocolLabels.demonDeath, demonId};
            string deathMsg = MessageMaker.makeMessage(msgParts);
            // HD.UDPChat.instance.Send(deathMsg);
            Destroy(gameObject);
        }
    }

    public float getDemonSpeed()
    {
        return demonSpeed;
    }

    public int getDemonAttackDamage()
    {
        return demonAttackDamage;
    }

    public void setDemonId(string value)
    {
       demonId = value;
    }
}