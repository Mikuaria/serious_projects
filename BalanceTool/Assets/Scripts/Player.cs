using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
	//[SerializeField] Enemy currentEnemy = null;

	GameManager gameManager;

	float camMoveSpeed = 50.0f;

	CharacterController controller = null;


	[Header("Controller")]
	Vector3 velocity;

	float gravity = -9.81f;
	public float jumpHeight = 3f;

	public Transform groundCheck = null;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;

	bool isGrounded = false;

	// Start is called before the first frame update
	protected override void Start()
	{
		gameManager = GameManager.instance;

		UIcc = GetComponent<UICharacterComponent>();

		controller = GetComponent<CharacterController>();
		base.Start();
	}

	// Update is called once per frame
	protected override void Update() 
	{
		base.Update();
		if (gameManager.currentToolState == ToolState.PLAY)
		{
			PlayInputs();
		}
		if (gameManager.currentToolState == ToolState.EDIT)
		{
			EditInputs();
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			gameManager.SwitchToolState();

			currentShootCooldown = 0.0f;
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			SaveData();
		}

		if (Input.GetKeyDown(KeyCode.F3))
		{
			if (!fileName.Equals(""))
			{
				LoadData();
			}
			else
			{
				GameManager.instance.ChangeInfoText("PlayerFilename is empty !");
			}
		}

		UIcc.currentAmmoCount.text = currAmmo.ToString() + " / " + data.maxAmmo + " bullets";
	}

	void PlayInputs()
	{ 
		if (currTimerReload>= data.reloadTime)
		{
			currAmmo = data.maxAmmo;

			currTimerReload = 0.0f;

			isReloading = false;
		}

		if (isReloading)
		{
			currTimerReload += Time.deltaTime;

			GameManager.instance.ChangeInfoText("Reloading !");
		}

		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y <0)
		{
			velocity.y = -2f;
		}

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;

		controller.Move(move * data.moveSpeed * Time.deltaTime);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			ExecuteSpell();
		}

		velocity.y += gravity * Time.deltaTime * 5f;

		controller.Move(velocity * Time.deltaTime);

		if (Input.GetMouseButton(0) && !isReloading && currAmmo>0)
		{
			if (currentShootCooldown>=1.0f/data.fireRate)
			{
				Shoot();
			}
		}

		if (currAmmo<= 0)
		{
			isReloading = true;
		}
	}

	void EditInputs()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + Camera.main.transform.forward * z;

		transform.position += move * camMoveSpeed * Time.deltaTime;

		if (Input.GetButton("Jump"))
		{
			transform.position += transform.up * Time.deltaTime * camMoveSpeed;
		}

		if (Input.GetButton("Crouch"))
		{
			transform.position -= transform.up * Time.deltaTime * camMoveSpeed;
		}

		if (camMoveSpeed + Input.mouseScrollDelta.y > 1.0f && camMoveSpeed + Input.mouseScrollDelta.y <= 100.0f)
		{
			camMoveSpeed += Input.mouseScrollDelta.y;
		}
	}
}
