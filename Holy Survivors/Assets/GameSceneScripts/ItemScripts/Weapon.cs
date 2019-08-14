using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int itemNo;
    public Animation anim;
    public Transform bulletSpawn;
    public bool isLoot;

    private string itemId;
    private Item weapon;
    private int weaponMode;

    private int baseDamage;
    private int powerAttackDamage;
    private float swingSpeed;
    private float loadSpeed;

    private CooldownScript cooldownScript;
    private Player localPlayer;
    public bool canUse;
    private bool powerAttack;

    private string weaponFilePath = "Prefabs/Weapons/";
    private GameObject musketBallPrefab;
    private GameObject pistovBallPrefab;
    
    void Start()
    {
        itemId = "w" + itemNo.ToString();
        
        weapon = new Item(itemId);
        
        gameObject.name = weapon.getItemName();
        
        switch(itemId)
        {
            case ItemId.musket:
                swingSpeed = 0.8f;
                loadSpeed = 0.4f;
                baseDamage = 10; // Bayonet Damage
                break;
            
            case ItemId.pistov:
                loadSpeed = 0.9f;
                break;

            case ItemId.woodenAxe:
                swingSpeed = 1.1f;
                baseDamage = 13;
                powerAttackDamage = 22;
                break; 

            case ItemId.spear:
                swingSpeed = 1.3f;
                baseDamage = 15;
                powerAttackDamage = 20;
                break;

            case ItemId.cutlass:
                swingSpeed = 1.5f;
                baseDamage = 12;
                powerAttackDamage = 18;
                break;

            default:
                break;       
        }
    
        musketBallPrefab = (GameObject) Resources.Load(weaponFilePath + "MusketBall");
        pistovBallPrefab = (GameObject) Resources.Load(weaponFilePath + "PistovBall");
    }

    void Update()
    {   
        // If weapon is loot, none of functions will be able to use, just giving infos to CameraRay
        if(!isLoot)
        {
            if(GameObject.Find("GameEditor").GetComponent<GameSceneEventHandler>().localPlayer
            && cooldownScript == null)
            {
                localPlayer = GameObject.Find("GameEditor").GetComponent<GameSceneEventHandler>().localPlayer;
                cooldownScript = new CooldownScript(itemId, localPlayer, swingSpeed, loadSpeed);

                canUse = true;
            }

            // Change weaponMode, NOTE: Not all weapons have second mode
            if(Input.GetKeyDown(KeyCode.X))
            {
                anim.Stop();

                if(weaponMode == 0)
                {
                    weaponMode = 1;
                }
                else
                {
                    weaponMode = 0;
                }
            }

            if(weaponMode == 0)
            {
                if(canUse)
                {
                    MouseFunctions.mouseClickActions(itemId, gameObject);
                }

                // Special Event of Firearms for late reloading
                if(!canUse)
                {
                    if(zoomed)
                    {
                        switch(itemId)
                        {
                            case ItemId.musket:
                                musketRightFunc();
                                break;
                            case ItemId.pistov:
                                pistovRightFunc();
                                break;
                            default:
                                break;        
                        }
                    }
                    else
                    {
                        if(!anim.isPlaying)
                        {
                            switch(itemId)
                            {
                                case ItemId.musket:
                                    reloadMusket();
                                    break;
                                case ItemId.pistov:
                                    reloadPistov();
                                    break;
                                default:
                                    break;
                            }
                        }
                    } 
                }
            }
            else
            {
                if(!anim.isPlaying)
                {
                    MouseFunctions.mouseClickActions(itemId, gameObject);
                }
            }
        }
    }

    // For melee attacks
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        
        if(other.GetComponent<Demon>())
        {
            if(powerAttack)
            {
                other.GetComponent<Demon>().setHP(other.GetComponent<Demon>().getHP() - powerAttackDamage);                    
            }
            else
            {
                other.GetComponent<Demon>().setHP(other.GetComponent<Demon>().getHP() - baseDamage);
            }

            Debug.Log(other.GetComponent<Demon>().getHP());
        }
    }

    #region OnClick Functions

    private string animFilePath = "Animations/Weapon/";

    // Musket Mouse Functions and Reload
    internal void musketLeftFunc()
    {
        if(weaponMode == 0)
        {
            // Fire
            if(!anim.isPlaying)
            {
                GameObject musketBall = Instantiate(musketBallPrefab, bulletSpawn.position, bulletSpawn.rotation);

                canUse = false;
            }

            reloadMusket();
        }
        else
        {
            // Bayonet attack
            anim.clip = (AnimationClip) Resources.Load(animFilePath + "Musket(1)LeftClick");
            anim[anim.clip.name].speed = cooldownScript.getCooldownRate(weaponMode);
            anim.Play();
        }
    }

    private bool zoomed = false;
    private float zoomAnimSpeed;

    internal void musketRightFunc()
    {
       if(weaponMode == 0)
        {
            // Zoom
            // NOTE: Animation will be changed or removed,
            //       camera itself will move to gun aim and maybe open aim UI
            anim.clip = (AnimationClip) Resources.Load(animFilePath + "Musket(0)RightClick");

            if(zoomed)
            {
                zoomAnimSpeed = -1;
                anim[anim.clip.name].time = anim[anim.clip.name].length;
                zoomed = false;
            }
            else
            {
                zoomAnimSpeed = 1;
                zoomed = true;
            }

            anim[anim.clip.name].speed = zoomAnimSpeed;
            anim.Play();
        }
        else
        {
            // Do nothing
        } 
    }

    private void reloadMusket()
    {
        anim.clip = (AnimationClip) Resources.Load(animFilePath + "MusketReload");
        anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
        anim.Play();
    }

    // Pistov Mouse Functions and Reload
    internal void pistovLeftFunc()
    {
        GameObject pistovBall = Instantiate(pistovBallPrefab, bulletSpawn.position, bulletSpawn.rotation);

        canUse = false;

        reloadPistov();
    }

    internal void pistovRightFunc()
    {
        anim.clip = (AnimationClip) Resources.Load(animFilePath + "PistovRightClick");

        if(zoomed)
        {
            zoomAnimSpeed = -1;
            anim[anim.clip.name].time = anim[anim.clip.name].length;
            zoomed = false;
        }
        else
        {
            zoomAnimSpeed = 1;
            zoomed = true;
        }

        anim[anim.clip.name].speed = zoomAnimSpeed;
        anim.Play();     
    }

    private void reloadPistov()
    {
        anim.clip = (AnimationClip) Resources.Load(animFilePath + "PistovReload");
        anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
        anim.Play();
    }
    
    // Cutlass Mouse Functions
    private float cutlassPowerAttackCost = 15;

    internal void cutlassLeftFunc()
    {
        powerAttack = false;
        anim.clip = (AnimationClip) Resources.Load(animFilePath + "CutlassRightClick");
        anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
        anim.Play(); 
    }

    internal void cutlassRightFunc()
    {
        if(localPlayer.getStamina() >= cutlassPowerAttackCost)
        {
            powerAttack = true;
            anim.clip = (AnimationClip) Resources.Load(animFilePath + "CutlassRightClick");
            anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
            anim.Play(); 
            localPlayer.setStamina( localPlayer.getStamina() - cutlassPowerAttackCost );
        }
    }

    // Wooden Axe Mouse Functions
    private float axePowerAttackCost = 25;

    internal void woodenAxeLeftFunc()
    {
        anim.clip = (AnimationClip) Resources.Load(animFilePath + "AxeLeftClick");
        anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
        anim.Play();
    }

    internal void woodenAxeRightFunc()
    {
        if(localPlayer.getStamina() >= axePowerAttackCost)
        {
            anim.clip = (AnimationClip) Resources.Load(animFilePath + "AxeRightClick");
            anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
            anim.Play(); 
            localPlayer.setStamina( localPlayer.getStamina() - axePowerAttackCost );
        } 
    }

    // Spear Mouse Functions
    private float spearPowerAttackCost = 20;

    internal void spearLeftFunc()
    {
        powerAttack = false;
        anim.clip = (AnimationClip) Resources.Load(animFilePath + "SpearLeftClick");
        anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
        anim.Play();
    }

    internal void spearRightFunc()
    {
        if(localPlayer.getStamina() >= spearPowerAttackCost)
        {
            powerAttack = true;
            anim.clip = (AnimationClip) Resources.Load(animFilePath + "SpearRightClick");
            anim[anim.clip.name].speed = cooldownScript.getCooldownRate();
            anim.Play(); 
            localPlayer.setStamina( localPlayer.getStamina() - spearPowerAttackCost );
        }
    }
    #endregion
}