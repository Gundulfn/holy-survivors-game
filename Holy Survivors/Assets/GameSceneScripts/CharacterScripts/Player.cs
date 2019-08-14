using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Character
{
	// Player Info
	public TextMeshPro nameText;
	private string roleName;

    // Movement variables
    /// Static variables
    public CharacterController charCont;
	
    private float walkSpeed;
    private float runSpeed;
    
    /// Non-static variables
	private float targetSpeed;
	
	private bool isPlayerMoving;
    private bool running;
	private bool canRun;
    
    private float currentSpeed;
	private float velocityY;

    // Stamina
    private float stamina;
	private float defaultStamina;
    private float staminaTime = 5;

    // Camera Rotation Vector3
	internal Vector3 v3Rotate = Vector3.zero;
    
    void Start()
    {   
		switch(roleName)
		{
			case "musketeer":
				setHP(100);
                setAttackSpeed(0.8f);
				setReloadSpeed(1.5f);
				
				walkSpeed = 5;
        		runSpeed = 11; 
				stamina = 90; 
                break;

            case "royalGuard":
				setHP(120);
				setAttackSpeed(1.6f);
				setReloadSpeed(0.6f);

				walkSpeed = 4;
        		runSpeed = 8; 
				stamina = 120;
                break;

            case "pirate":
				setHP(110);
				setAttackSpeed(1.4f);
				setReloadSpeed(1.2f);

				walkSpeed = 5;
        		runSpeed = 9; 
				stamina = 100;
                break;

            case "lumberjack":
				setHP(130);
				setAttackSpeed(1.2f);
				setReloadSpeed(0.4f);

				walkSpeed = 5;
        		runSpeed = 8; 
				stamina = 110;
                break;

            default:
                break;	
		}
        
		defaultStamina = stamina;
        setMoveSpeed(walkSpeed);  
    }

    void Update ()
	{		
		if(canRun)
		{
			running = Input.GetKey(KeyCode.LeftShift);
		}else
		{
			running = false;
		}	

		if(charCont.velocity != Vector3.zero)
		{
			isPlayerMoving = true;
		}
		else
		{
			isPlayerMoving = false;
		}

        // Calculations
        StaminaStates();
        MoveCalc();
	}

    // Physic and Movement Calculation Part
    void MoveCalc()
	{
		targetSpeed = ((running) ? runSpeed : walkSpeed);
        
        float gravity = getPhysicVar(0);
        float speedSmoothTime = getPhysicVar(2);
        float speedSmoothVelocity = getPhysicVar(3);
		
        currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, 
                                                    GetModifiedSmoothTime(speedSmoothTime));
		
        
		velocityY += Time.deltaTime * gravity;

		Vector3 velocityGravity = transform.right * 0 + velocityY * Vector3.up;
		charCont.Move (velocityGravity * Time.deltaTime);
		
		//currentSpeed = new Vector3 (charCont.velocity.x,  0, charCont.velocity.z).magnitude;

		if (charCont.isGrounded)
		{
			velocityY = 0;
		}
	}

    float GetModifiedSmoothTime(float smoothTime)
	{
        float airControlPercent = getPhysicVar(1);

		if (charCont.isGrounded)
		{
			return smoothTime;
		}

		if (airControlPercent == 0)
		{
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

    // Stamina Part
	private void StaminaStates()
	{
		if(stamina <= 0)
		{
			stamina = 0;
			canRun = false;

		}else if(stamina >= defaultStamina)
		{
			stamina = defaultStamina;
		}
		
		if(stamina >= 5)
		{
			canRun = true;
		}
	}
	
    // Stamina Calculation Part
	internal void StaminaChanges()
	{
		float timeLimit;

		if(isPlayerMoving && running)
		{
			stamina -= 20 * Time.deltaTime;
			staminaTime = 0;
		}
		else if(stamina < defaultStamina)
		{
			//Delay Time due to Remaining Stamina
			if(stamina != 0)
			{
				timeLimit = 3;
			}else
			{
				timeLimit = 5;
			}

			if(staminaTime >= timeLimit)
			{
				stamina += 5 * Time.deltaTime;
			}else
			{
				staminaTime += Time.deltaTime;
			}
		}
	}

    // Get and Set variables part

    /// stamina
    public float getStamina()
    {
        return stamina;
    }   

    public void setStamina(float value)
    {
        stamina = value;
		
		StaminaChanges();
    }

    /// runSpeed
    public float getRunSpeed()
    {
        return runSpeed;
    }

    /// currentSpeed
    public float getCurrentSpeed()
    {
        return currentSpeed;
    }

    /// velocityY
    public float getVelocityY()
    {
        return velocityY;
    }

	/// roleName
	public string getRoleName()
	{
		return roleName;
	}

	public void setRoleName(string value)
	{
		roleName = value;
	}

	/// isPlayerMoving
	public bool getIsPlayerMoving()
	{
		return isPlayerMoving;
	}
}