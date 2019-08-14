using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    // Attributes
    private string charName;
    private int hp = 100;
    private float moveSpeed;
	private float attackSpeed;
	private float reloadSpeed;

    // Physic variables

    private float gravity = -50;
    private float airControlPercent = 0.2f;
	private float speedSmoothTime = 0.1f;
	private float speedSmoothVelocity = 0.2f;
    
    private float[] physicVariables;

    void Start()
    {
          
    }
    
    public Character(){ }

    public Character(string playerName, int playerHP)
    {
        charName = playerName;
        hp = playerHP;
    }

    // physic variables get and set

    public float getPhysicVar(int index)
    {
        physicVariables = new float[4]{gravity, airControlPercent, speedSmoothTime, speedSmoothVelocity};
        
        return physicVariables[index];
    }

    public void setPhysicVar(int index, float value)
    {
        physicVariables[index] = value;
    }

    // name get and Set 
    public string getName()
    {
        return charName;
    }

    public void setName(string value)
    {
        charName = value;
    }

    // hp get and Set 
    public int getHP()
    {
        return hp;
    }

    public void setHP(int value)
    {
        hp = value;
    }

    // moveSpeed get and Set 
    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public void setMoveSpeed(float value)
    {
        moveSpeed = value;
    }

    // attackSpeed get and Set 
    public float getAttackSpeed()
    {
        return attackSpeed;
    }

    public void setAttackSpeed(float value)
    {
        attackSpeed = value;
    }

    // reloadSpeed get and Set 
    public float getReloadSpeed()
    {
        return reloadSpeed;
    }

    public void setReloadSpeed(float value)
    {
        reloadSpeed = value;
    }
}