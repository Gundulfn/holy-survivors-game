using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownScript
{
    private float cooldownRate;
    private Player player;
    private float swingSpeed;
    private float loadSpeed;

    public CooldownScript(string itemId, Player player, 
                          float swingSpeed, float loadSpeed)
    { 
        this.player = player;
        this.swingSpeed = swingSpeed;
        this.loadSpeed = loadSpeed;

        if(itemId == ItemId.musket || itemId == ItemId.pistov)
        {
            cooldownRate = player.getReloadSpeed() * loadSpeed;
        }
        else
        {
            cooldownRate = player.getAttackSpeed() * swingSpeed;
        }
        
    }

    public CooldownScript()
    {
        cooldownRate = 1;    
    }

    // @param weaponMode 0 for ranged 1 for melee mode
    // @returns float cooldownRate for weapon with weaponMode
    public float getCooldownRate(int weaponMode = 0)
    {
        if(weaponMode == 0)
        {
            return cooldownRate;      
        }
        else
        {
            return player.getAttackSpeed() * swingSpeed;
        }

        
    } 
}