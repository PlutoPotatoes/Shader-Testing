using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using System.Collections;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;
		private float gravityReset;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -1f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;
		[Tooltip("Active Player Camera")]
		public Camera mainCam;

		[Header("Misc")]
		[Tooltip("Layers to Detect for Interaction")]
		public LayerMask interactLayers;
		[Tooltip("Player Respawn Point")]
		public Transform respawnPoint;
		[Tooltip("Player Dash Speed")]
		public float dashSpeed;
		[Tooltip("Player Dash Length")]
		public float dashLength;
		[Tooltip("Player Animator")]
		public Animator animator;



		//testing vars

		public GameObject playerCapsule;
		public float rotationSpeed;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private Vector3 inputDirection;
		private float dashCooldownTimer;
		private const float dashCooldown = 0.5f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;
		private bool canMove;
		private Vector3 correctedDir;

		//animator
		private enum animatorState{
			IDLE,
			WALKING,
			RUNNING,
			ROLLING,
			DYING
        };
		private animatorState playerState;
		

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			gravityReset = Gravity;	
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
			canMove = true;
			playerState = animatorState.IDLE;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			dash();
			if(transform.position.y < -16)
            {
				respawn();
            }
			cooldowns();
			animationHandler();
		}

		private void animationHandler()
        {
            switch (playerState)
            {
				case animatorState.IDLE:
					animator.SetBool("WalkTrigger", false);
					animator.SetBool("RunTrigger", false);
					animator.SetBool("RollTrigger", false);
					animator.SetBool("IdleTrigger", true);
					animator.SetBool("DeathTrigger", false);

					break;
				case animatorState.RUNNING:
					animator.SetBool("WalkTrigger", false);
					animator.SetBool("RunTrigger", true);
					animator.SetBool("IdleTrigger", false);
					animator.SetBool("RollTrigger", false);
					animator.SetBool("DeathTrigger", false);


					break;
				case animatorState.WALKING:
					animator.SetBool("WalkTrigger", true);
					animator.SetBool("RunTrigger", false);
					animator.SetBool("IdleTrigger", false);
					animator.SetBool("RollTrigger", false);
					animator.SetBool("DeathTrigger", false);


					break;
				case animatorState.ROLLING:
					animator.SetBool("RollTrigger", true);
					animator.SetBool("IdleTrigger", false);
					animator.SetBool("DeathTrigger", false);

					break;

				case animatorState.DYING:
					animator.SetBool("WalkTrigger", false);
					animator.SetBool("RunTrigger", false);
					animator.SetBool("RollTrigger", false);
					animator.SetBool("IdleTrigger", false);
					animator.SetBool("DeathTrigger", true);
					break;


			}
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (_input.look.sqrMagnitude >= _threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
				_rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

				// Update Cinemachine camera target pitch
				CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;



			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero)
			{
				targetSpeed = 0.0f;
				if(playerState != animatorState.DYING) playerState = animatorState.IDLE;
			}

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			Vector3 rotDir = new Vector3(_input.move.x, 0, _input.move.y);
			

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				// move
				Quaternion offset = Quaternion.AngleAxis(45f, Vector3.up);
				rotDir = offset * rotDir;
				correctedDir = rotDir;
				Quaternion newRotation = Quaternion.LookRotation(rotDir.normalized, Vector3.up);
				if (canMove)
				{
					playerCapsule.transform.rotation = Quaternion.RotateTowards(playerCapsule.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
					if (_input.sprint)
					{
						playerState = animatorState.RUNNING;
                    }
                    else
                    {
						playerState = animatorState.WALKING;

					}
				}


			}

			// move the player
			if (canMove)
			{
				_controller.Move(rotDir.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
            else
            {
				_controller.Move(new Vector3(0,0,0));

			}

		}


		private void dash()
        {
			if (Grounded)
			{
				if (_input.move != Vector2.zero && Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0)
				{
					StartCoroutine(dashHandler());
					dashCooldownTimer = dashCooldown;
				}
			}
        }

		IEnumerator dashHandler()
        {
			Gravity = 0f;
			float startTime = Time.time;
			playerState = animatorState.ROLLING;
			while (Time.time < startTime + dashLength)
            {
				_controller.Move(correctedDir * dashSpeed * Time.deltaTime);

				yield return null;
            }
			Gravity = gravityReset;
			playerState = animatorState.WALKING;


		}

		private void cooldowns()
        {
			if(dashCooldownTimer > 0)
            {
				dashCooldownTimer -= Time.deltaTime;
            }
        }
		private void JumpAndGravity()
		{

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (!Grounded && _verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
            }
            else if(Grounded)
            {
				_verticalVelocity = 0;
            }
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		public void respawn()
		{
			StartCoroutine(Stun(1));
			print("die");
		}

		IEnumerator Stun(float time)
        {
			canMove = false;
			transform.position = respawnPoint.position;
			playerState = animatorState.DYING;
			yield return new WaitForSeconds(2.5f);
			playerState = animatorState.IDLE;
			canMove = true;
        }
	}

	

}