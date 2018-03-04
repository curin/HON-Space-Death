using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSController : MonoBehaviour
{
	public enum CharacterState
	{
		Grounded,
		Flying
	};

	public Animator animator;
	public GravityAttractor planet;

	public CharacterState state = CharacterState.Flying;
	public float Deceleration = .5f;
    public float MoveAcceleration = 2f;
    public float RunModifier = 2.0f;
    public float MaxSpeed = 10.0f;
    public bool toggleRun = false;
	public bool OnPlanet = false;

    public Vector3 MoveDirection = Vector3.zero;
	public float mouseSensitivity = 100.0f;
	public Camera mainCam;
	public float zoomSpeed = 2;
	public float angleClamp = 80;
	public float zRotMax = 3;
	public float zRotDec = .3f;
	public float zRotRev = .5f;

	private float zoomFov = 35;
	private float regFov = 50;
	private float currentFov = 50;


	private bool isZoom = false;

#if DEBUG
    public Vector3 velocity = Vector3.zero;
    public float movementModifier = 1;
	public Vector3 ActualVelocity = Vector3.zero;
	public float Speed = 1;
	public Vector3 CameraRot = Vector3.zero;
#else
	private Vector3 CameraRot = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private float movementModifier = 1;
	private Vector3 ActualVelocity = Vector3.zero;
	private float Speed = 1;
#endif

	public Rigidbody body;
	private bool runRecent = false;
	public int runDown = 0;

    // Use this for initialization
    void Start()
    {
		mainCam = Camera.main;
		body = GetComponent<Rigidbody>();
		CameraRot = transform.localRotation.eulerAngles;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		animator = GetComponentInChildren<Animator> ();

		// Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
		body.useGravity = false;
		body.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
		movement ();
		mouseLook ();
    }

    // Update is called once per frame
    void Update()
    {
		mouseZoom ();
		animator.SetBool ("zoom", isZoom);

        if (toggleRun && Input.GetButtonDown("Run"))
            movementModifier = (movementModifier == 1 ? RunModifier : 1);
    }
		
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Planet")
			OnPlanet = true;
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.tag == "Planet")
			OnPlanet = false;
	}

	void movement()
	{
		float inputX = Input.GetAxis ("Horizontal");
		float inputZ = Input.GetAxis ("Vertical");
		float inputY = Input.GetAxis ("Jump");
		velocity = body.velocity;
		Speed = velocity.magnitude;

		if (!toggleRun)
			movementModifier = (Input.GetButton ("Run") ? RunModifier : 1);

		MoveDirection = new Vector3 (inputX, inputY, inputZ);
		if (state == CharacterState.Grounded)
			MoveDirection = mainCam.transform.localToWorldMatrix.MultiplyVector(MoveDirection);
		else
			MoveDirection = transform.localToWorldMatrix.MultiplyVector (MoveDirection);
		MoveDirection.Normalize ();

		if (OnPlanet)
			MoveDirection.y *= 10;

		ActualVelocity = velocity * movementModifier * MoveAcceleration * Time.deltaTime;

		body.AddForce (MoveDirection * movementModifier * MoveAcceleration * Time.deltaTime, ForceMode.VelocityChange);

		if (body.velocity.magnitude > 0) 
		{
			if (body.velocity.magnitude < Deceleration * Time.deltaTime)
				body.AddForce (-body.velocity, ForceMode.VelocityChange);
			else 
			{
				velocity = -body.velocity;
				velocity.Normalize ();
				if (OnPlanet)
					body.AddForce (velocity * Deceleration * 2 * Time.deltaTime, ForceMode.VelocityChange);
				else
					body.AddForce (velocity * Deceleration * Time.deltaTime, ForceMode.VelocityChange);
				velocity = body.velocity;
			}
		}

		if (body.velocity.magnitude > MaxSpeed && movementModifier == 1 && !runRecent) {
			body.AddForce (-body.velocity, ForceMode.VelocityChange);
			velocity.Normalize ();
			velocity *= MaxSpeed;
			body.AddForce (velocity, ForceMode.VelocityChange);
			velocity = body.velocity;
		} else if (body.velocity.magnitude > MaxSpeed * movementModifier && movementModifier > 1) {
			runRecent = true;
			runDown++;
			if (movementModifier > 1)
				runDown = 0;
			body.AddForce (-body.velocity, ForceMode.VelocityChange);
			velocity.Normalize ();
			velocity *= MaxSpeed * (movementModifier - (runDown * Time.deltaTime));
			body.AddForce (velocity, ForceMode.VelocityChange);
			velocity = body.velocity;
		} 
		else if (body.velocity.magnitude > MaxSpeed) {
			runRecent = true;
			if (movementModifier == 1)
				body.AddForce (-MoveDirection * MoveAcceleration * Time.deltaTime, ForceMode.VelocityChange);
			runDown++;
			if (movementModifier > 1)
				runDown = 0;
		}
		else if (runRecent) {
			runRecent = false;
			runDown = 0;
		}

		if (state == CharacterState.Grounded && !OnPlanet) 
		{
			planet.Attract (body, false);
		} 
	}
		
	void mouseLook()
	{
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");
		float mouseZ = Input.GetAxis ("Rotate");

		CameraRot.y = mouseX * mouseSensitivity * Time.deltaTime;


		if (state == CharacterState.Grounded) 
		{
			CameraRot.x = Mathf.Clamp (mouseY * mouseSensitivity * Time.deltaTime, -angleClamp, angleClamp);
		} 
		else 
		{
			CameraRot.x = mouseY * mouseSensitivity * Time.deltaTime;
			CameraRot.z += mouseZ * zRotRev * Time.deltaTime;
		}

		CameraRot.z = Mathf.Clamp (CameraRot.z, -zRotMax, zRotMax);

		if (CameraRot.z < zRotDec * Time.deltaTime && CameraRot.z > -zRotDec * Time.deltaTime && CameraRot.z != 0)
			CameraRot.z = 0;
		else if (CameraRot.z != 0)
			CameraRot.z += (CameraRot.z < 0 ? 1 : -1) * zRotDec * Time.deltaTime;
		
		Quaternion localRotation = Quaternion.Euler (CameraRot);

		if (OnPlanet)
			mainCam.transform.rotation *= localRotation;
		else
			transform.rotation *= localRotation;
	}

	void mouseZoom()
	{
		mainCam.fieldOfView = currentFov;

		if (Input.GetAxis("Fire2") > 0) {
			isZoom = true;
			if (currentFov > zoomFov) 
			{
				currentFov = currentFov - zoomSpeed;
			}
		} 
		else 
		{
			isZoom = false;
			if (currentFov < regFov)
			{
				currentFov = currentFov + zoomSpeed;
			}
		}
	}
}
