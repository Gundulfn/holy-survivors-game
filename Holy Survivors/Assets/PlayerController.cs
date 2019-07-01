using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {
	
	public bool enableControl;
	public float stamina;
	
	//Physic Settings
	public float walkSpeed;
	public float runSpeed;
	public float targetSpeed;
	public float gravity;
	public float airControlPercent;
	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;
	float velocityY;
	bool running;
	bool canRun;

	// Other Settings
	float staminaTime = 0;
	CharacterController controller;
	//Camera Settings
	public GameObject Cam;
	Vector3 v3Rotate = Vector3.zero;

	void Start ()
	{
		controller = GetComponent<CharacterController> ();
		Cam.transform.localEulerAngles = v3Rotate;
		
		float rot = PlayerPrefs.GetFloat("checkRotation", 0); 

		v3Rotate.y = rot;
	}

	void Update ()
	{

		// Input	
		Vector3 inputDir = new Vector3 (Input.GetAxis("Horizontal"),  0, Input.GetAxis("Vertical"));
		
		if(canRun)
		{
			running = Input.GetKeyDown(KeyCode.LeftShift);
		}else
		{
			running = false;
		}
		
		//Main Functions
		StaminaStates();
		Move (inputDir, running);
		CameraRotate();		
	}
	// Camera Part
 	void CameraRotate()
	 {
		
		if(enableControl)
		{
			
			v3Rotate.x += Input.GetAxis("Mouse Y") * 50 * Time.deltaTime * -1;
			v3Rotate.x = Mathf.Clamp(v3Rotate.x, -90, 90);

			v3Rotate.y += Input.GetAxis("Mouse X") * 50 * Time.deltaTime;

			transform.eulerAngles = new Vector3(0, v3Rotate.y, 0);
			Cam.transform.eulerAngles = new Vector3(v3Rotate.x, transform.eulerAngles.y, 0);
		}
	}

	// Movement Part
	void Move(Vector3 inputDir, bool running)
	{
			
		targetSpeed = ((running) ? runSpeed : walkSpeed);
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));
		
		velocityY += Time.deltaTime * gravity;
		MovementControl(transform);

		Vector3 velocityGravity = transform.right * 0 + velocityY * Vector3.up;
		controller.Move (velocityGravity * Time.deltaTime);
		
		currentSpeed = new Vector3 (controller.velocity.x,  0, controller.velocity.z).magnitude;

		if (controller.isGrounded)
		{
			velocityY = 0;
		}
	}

	/// Movement Buttons
	void MovementControl(Transform objTransform)
	{
		int negative = -1;
		bool isPlayerMoving = false;
		
		if(enableControl)
		{
			if(Input.GetKey(KeyCode.W))
			{
				Vector3 velocityForward = objTransform.forward * currentSpeed + velocityY * Vector3.up;
				controller.Move(velocityForward * Time.deltaTime);
				isPlayerMoving = true;
			}

			if(Input.GetKey(KeyCode.A))
			{
				Vector3 velocityLeft = objTransform.right * negative * currentSpeed + velocityY * Vector3.up;
				controller.Move(velocityLeft * Time.deltaTime);		
				isPlayerMoving = true;
			}

			if(Input.GetKey(KeyCode.S))
			{
				Vector3 velocityBackward = objTransform.forward * negative * currentSpeed / 2f + velocityY * Vector3.up;
				controller.Move(velocityBackward * Time.deltaTime);
				isPlayerMoving = true;
			}

			if(Input.GetKey(KeyCode.D))
			{
				Vector3 velocityRight = objTransform.right * currentSpeed + velocityY * Vector3.up;
				controller.Move(velocityRight * Time.deltaTime);
				isPlayerMoving = true;
			}
			
			StaminaChanges(isPlayerMoving);					
		}
	}
	float GetModifiedSmoothTime(float smoothTime)
	{
		if (controller.isGrounded)
		{
			return smoothTime;
		}

		if (airControlPercent == 0)
		{
			Debug.Log(float.MaxValue);
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}

	// Stamina Part
	void StaminaStates()
	{
		if(stamina <= 0)
		{
			stamina = 0;
			canRun = false;
		}else if(stamina >= 100)
		{
			stamina = 100;
		}
		///
		if(stamina >= 5)
		{
			canRun = true;
		}
	}
	void StaminaChanges(bool isPlayerMoving){
		float timeLimit;

		if(isPlayerMoving && running)
		{
			stamina -= 20 * Time.deltaTime;
			staminaTime = 0;
		}
		else if(stamina < 100)
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

}