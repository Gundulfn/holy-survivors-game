using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour {
	
	public bool enableControl;
	private float hp;
	private float stamina;

	Player player;
	public GameObject Cam;
	CharacterController controller;
	Vector3 v3Rotate;
	
	void Start ()
	{
		player = GetComponent<Player>();
		
		hp = player.getHP();
		stamina = player.getStamina();
		controller = player.charCont;

		v3Rotate = player.v3Rotate;
		Cam.transform.localEulerAngles = v3Rotate;
	}

	void Update ()
	{
		MovementControl(transform);
		CameraRotate();		
	}
	
	// Camera Part
 	void CameraRotate()
	{
		if(enableControl)
		{
			if(Input.GetAxis("Mouse X") != 0)
			{
				sendRot();
			}

			v3Rotate.x += Input.GetAxis("Mouse Y") * 50 * Time.deltaTime * -1;
			v3Rotate.x = Mathf.Clamp(v3Rotate.x, -90, 90);

			v3Rotate.y += Input.GetAxis("Mouse X") * 50 * Time.deltaTime;

			transform.eulerAngles = new Vector3(0, v3Rotate.y, 0);
			Cam.transform.eulerAngles = new Vector3(v3Rotate.x, transform.eulerAngles.y, 0);
		}
	}

	/// Movement Buttons
	void MovementControl(Transform objTransform)
	{
		int negative = -1;
		
		if(enableControl)
		{
			float currentSpeed = player.getCurrentSpeed();
			float velocityY = player.getVelocityY();
			
			if(Input.GetKey(KeyCode.W))
			{
				Vector3 velocityForward = objTransform.forward * currentSpeed + velocityY * Vector3.up;
				controller.Move(velocityForward * Time.deltaTime);
			}

			if(Input.GetKey(KeyCode.A))
			{
				Vector3 velocityLeft = objTransform.right * negative * currentSpeed + velocityY * Vector3.up;
				controller.Move(velocityLeft * Time.deltaTime);		
			}

			if(Input.GetKey(KeyCode.S))
			{
				Vector3 velocityBackward = objTransform.forward * negative * currentSpeed / 2f + velocityY * Vector3.up;
				controller.Move(velocityBackward * Time.deltaTime);
			}

			if(Input.GetKey(KeyCode.D))
			{
				Vector3 velocityRight = objTransform.right * currentSpeed + velocityY * Vector3.up;
				controller.Move(velocityRight * Time.deltaTime);
			}
			
			if(player.getIsPlayerMoving())
			{
				sendPos();
			}
				
		}

		player.StaminaChanges();
	}
	
	private void sendPos()
	{
		object[] msgParts = new object[5]{HD.ProtocolLabels.playerMove, 
										  HD.UDPChat.clientNo, 
										  transform.position.x,
										  transform.position.y,
										  transform.position.z,};

		string posMsg = MessageMaker.makeMessage(msgParts);
		
		//HD.UDPChat.instance.Send(posMsg);
	}

	private void sendRot()
	{
		object[] msgParts = new object[4]{HD.ProtocolLabels.playerRot, 
										  HD.UDPChat.clientNo, 
										  v3Rotate.x,
										  v3Rotate.y};

		string rotMsg = MessageMaker.makeMessage(msgParts);
		
		//HD.UDPChat.instance.Send(rotMsg);
	}
}