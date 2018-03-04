using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class FPSController : MonoBehaviour
{
	public enum CharacterState
	{
		Grounded,
		Flying
	};

	Animator animator;

	public CharacterState state = CharacterState.Flying;
	public float Deceleration = .1f;
    public float MoveAcceleration = 2.0f;
    public float RunModifier = 2.0f;
    public float MaxSpeed = 10.0f;
    public bool toggleRun = false;

    public Vector3 MoveDirection = Vector3.zero;
	public float mouseSensitivity = 100.0f;
	public Camera mainCam;
	public float zoomSpeed;
	public float angleClamp = 80;


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

	private Rigidbody body;
	private CapsuleCollider col;
	private bool runRecent = false;
	public int runDown = 0;
	private Quaternion savedRotation;

    // Use this for initialization
    void Start()
    {
		body = GetComponent<Rigidbody>();
		col = GetComponent<CapsuleCollider> ();
		CameraRot = transform.localRotation.eulerAngles;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		animator = GetComponentInChildren<Animator> ();
		savedRotation = transform.rotation;
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

	void movement(){
		float inputX = Input.GetAxis("Horizontal");
		float inputZ = Input.GetAxis("Vertical");
		float inputY = Input.GetAxis("Jump");
		velocity = body.velocity;
		Speed = velocity.magnitude;

		if (!toggleRun)
			movementModifier = (Input.GetButton ("Run") ? RunModifier : 1);

		MoveDirection = new Vector3(inputX, inputY, inputZ);
		MoveDirection = transform.localToWorldMatrix.MultiplyVector (MoveDirection);
		MoveDirection.Normalize();

		if (body.velocity.magnitude > MaxSpeed && movementModifier == 1 && !runRecent)
		{
			body.AddForce (-body.velocity, ForceMode.VelocityChange);
			velocity.Normalize ();
			velocity *= MaxSpeed;
			body.AddForce (velocity, ForceMode.VelocityChange);
			velocity = body.velocity;
		} 
		else if (body.velocity.magnitude > MaxSpeed && movementModifier > 1) 
		{
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
		else if (runRecent) 
		{
			runRecent = false;
			runDown = 0;
		}

		ActualVelocity = velocity * movementModifier * Time.deltaTime;

		body.AddForce(MoveDirection * MoveAcceleration, ForceMode.VelocityChange);

		if (body.velocity.magnitude > 0) 
		{
			if (body.velocity.magnitude < 1)
				body.AddForce (-body.velocity, ForceMode.VelocityChange);
			else {
				velocity = -body.velocity;
				velocity.Normalize ();
				body.AddForce (velocity, ForceMode.VelocityChange);
				velocity = body.velocity;
			}
		}
	}
		
	void mouseLook(){
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");
		float mouseZ = Input.GetAxis ("Rotate");

		CameraRot.y = mouseX * mouseSensitivity * Time.deltaTime;
		CameraRot.x = mouseY * mouseSensitivity * Time.deltaTime;

		if (state == CharacterState.Grounded)
			CameraRot.x = Mathf.Clamp (CameraRot.x, -angleClamp, angleClamp);
		else
			CameraRot.z = mouseZ * mouseSensitivity * Time.deltaTime;

		Quaternion localRotation = Quaternion.Euler (CameraRot);

		savedRotation *= localRotation;
		transform.rotation = savedRotation;
	}

	void mouseZoom(){


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
