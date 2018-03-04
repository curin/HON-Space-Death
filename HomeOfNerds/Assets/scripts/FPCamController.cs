using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPCamController : MonoBehaviour 
{
	public enum CharacterState
	{
		Grounded,
		Flying
	};

	public Rigidbody body;
	public Animator animator;
	public GravityAttractor planet;
	public Camera FPCam;

	public CharacterState State = CharacterState.Flying;
	public float Deceleration = .5f;
	public float MoveAcceleration = 2.0f;
	public float RunModifier = 2.0f;
	public float MaxSpeed = 10.0f;
	public float Speed = 0;
	public bool ToggleRun = false;
	public float MouseSensitivity = 150.0f;
	public float zRotMax = 3;
	public float zRotDec = .3f;
	public float zRotRev = .5f;
	public bool IsZoom = false;
	public float ZoomSpeed = 2;

	private Vector3 _velocity;
	private Quaternion SavedRot;
	private float zoomFov = 35;
	private float regFov = 50;
	private float currentFov = 50;
	private bool _runRecent = false;
	private int _runDown = 0;

	#if DEBUG
	public Vector3 _cameraRot = Vector3.zero;
	public Vector3 _moveVector;
	public float _movementModifier;
	public bool _onPlanet;
	#else
	private Vector3 _cameraRot = Vector3.zero;
	private Vector3 _moveVector;
	private float _movementModifier;
	private bool _onPlanet;
	#endif

	// Use this for initialization
	void Start () 
	{
		body = GetComponent<Rigidbody> ();
		animator = GetComponentInChildren<Animator> ();
		SavedRot = FPCam.transform.rotation;
		body.useGravity = false;
		body.constraints = RigidbodyConstraints.FreezeRotation;


	}

	void FixedUpdate()
	{
		Move ();
		Look ();
	}

	// Update is called once per frame
	void Update () 
	{
		//mouseZoom ();
		//animator.SetBool ("zoom", IsZoom);

		if (ToggleRun && Input.GetButtonDown("Run"))
			_movementModifier = (_movementModifier == 1 ? RunModifier : 1);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Planet")
			_onPlanet = true;
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.tag == "Planet")
			_onPlanet = false;
	}

	void Move()
	{
		Speed = _velocity.magnitude;

		_moveVector = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		_moveVector = FPCam.transform.localToWorldMatrix.MultiplyVector (_moveVector).normalized;

		body.AddForce (_moveVector * _movementModifier * MoveAcceleration * Time.deltaTime, ForceMode.VelocityChange);

		_moveVector = new Vector3 (0, Input.GetAxis ("Jump"), 0);

		body.AddForce (_moveVector * 1.6f * _movementModifier * MoveAcceleration * Time.deltaTime, ForceMode.VelocityChange);

		_moveVector = new Vector3(Input.GetAxis ("Horizontal"), Input.GetAxis ("Jump"), Input.GetAxis ("Vertical"));

		if (!ToggleRun)
			_movementModifier = (Input.GetButton ("Run") ? RunModifier : 1);

		if (body.velocity.magnitude > 0) 
		{
			if (body.velocity.magnitude < Deceleration * Time.deltaTime)
				body.AddForce (-body.velocity, ForceMode.VelocityChange);
			else 
			{
				_velocity = -body.velocity;
				_velocity.Normalize ();
				body.AddForce (_velocity * Deceleration * Time.deltaTime, ForceMode.VelocityChange);
				_velocity = body.velocity;
			}
		}

		if (body.velocity.magnitude > MaxSpeed && _movementModifier == 1 && !_runRecent) 
		{
			body.AddForce (-body.velocity, ForceMode.VelocityChange);
			_velocity.Normalize ();
			_velocity *= MaxSpeed;
			body.AddForce (_velocity, ForceMode.VelocityChange);
			_velocity = body.velocity;
		} 
		else if (body.velocity.magnitude > MaxSpeed * _movementModifier && _movementModifier > 1) 
		{
			_runRecent = true;
			_runDown++;
			if (_movementModifier > 1)
				_runDown = 0;
			body.AddForce (-body.velocity, ForceMode.VelocityChange);
			_velocity.Normalize ();
			_velocity *= MaxSpeed * (_movementModifier - (_runDown * Time.deltaTime));
			body.AddForce (_velocity, ForceMode.VelocityChange);
			_velocity = body.velocity;
		} 
		else if (body.velocity.magnitude > MaxSpeed) 
		{
			_runRecent = true;
			if (_movementModifier == 1)
				body.AddForce (-_moveVector * MoveAcceleration * Time.deltaTime, ForceMode.VelocityChange);
			_runDown++;
			if (_movementModifier > 1)
				_runDown = 0;
		}
		else if (_runRecent) 
		{
			_runRecent = false;
			_runDown = 0;
		}

		if (State == CharacterState.Grounded && !_onPlanet) 
		{
			planet.Attract (body, false);
		} 
	}

	void Look()
	{
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");
		float mouseZ = Input.GetAxis ("Rotate");

		_cameraRot.x = mouseY * MouseSensitivity * Time.deltaTime;
		_cameraRot.y = mouseX * MouseSensitivity * Time.deltaTime;

		if (State == CharacterState.Flying)
			_cameraRot.z += mouseZ * zRotRev * Time.deltaTime;
		_cameraRot.z = Mathf.Clamp (_cameraRot.z, -zRotMax, zRotMax);

		if (_cameraRot.z < zRotDec * Time.deltaTime && _cameraRot.z > -zRotDec * Time.deltaTime && _cameraRot.z != 0)
			_cameraRot.z = 0;
		else if (_cameraRot.z != 0)
			_cameraRot.z += (_cameraRot.z < 0 ? 1 : -1) * zRotDec * Time.deltaTime;

		Quaternion localRot = Quaternion.Euler (_cameraRot);

		SavedRot *= localRot;

		FPCam.transform.rotation *= localRot;
		//transform.rotation = FPCam.transform.rotation;
	}

	void mouseZoom()
	{
		FPCam.fieldOfView = currentFov;

		if (Input.GetAxis("Fire2") > 0) {
			IsZoom = true;
			if (currentFov > zoomFov) 
			{
				currentFov = currentFov - ZoomSpeed;
			}
		} 
		else 
		{
			IsZoom = false;
			if (currentFov < regFov)
			{
				currentFov = currentFov + ZoomSpeed;
			}
		}
	}
}
