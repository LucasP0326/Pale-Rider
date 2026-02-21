using UnityEngine;
using Articy.Unity; // Import Articy namespace
using Articy.Unity.Interfaces;
using Articy.Pale_Rider;
using Articy.Pale_Rider.GlobalVariables;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("UI")]
        public bool paused = false;
        public bool inMenu = false;
        public bool inDialogue = false;
        public bool movementEnabled = true;
        public GameObject pauseMenu;
        public GameObject dialogueManager;
        public GameObject thoughtCircle;
        public GameObject HUD;

        [Header("Thought")]
        public bool hasThought = false;
        public GameObject tempThoughtTrigger;
        public GameObject reptilianThoughtBubble;
        public GameObject paleomammalianThoughtBubble;
        public GameObject neomammalianThoughtBubble;
        public GameObject paleThoughtBubble;

        [Header("Player")]
        public bool isMounted = false;
        public bool isFalling = false;
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Interactables")]
        [Tooltip("What Interactable Object Player Currently Heads To")]
        public GameObject tempInteractableObject;
        public float interactionRange = 2f; // Maximum distance for interaction
        //public LayerMask InteractableLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        //Click Movement
        private Vector3 _targetPosition;
        public bool _isMovingToClick = false;
        private float _lastClickTime = 0f;
        private float _doubleClickThreshold = 0.3f;
        private bool _isSprintingToClick = false;
        [SerializeField] private GameObject clickIndicatorPrefab; // Assign in Inspector

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        public Animator _animator;
        public CharacterController _controller;
        public StarterAssetsInputs _input;
        public GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

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
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            dialogueManager = GameObject.FindObjectsByType<DialogueManager>(FindObjectsSortMode.None)[0].gameObject; // Find Dialogue Manager in the scene
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            // Retrieve the spawn point ID from PlayerPrefs
            string spawnPointID = ArticyGlobalVariables.Default.GlobalVariables.SpawnPoint;
            if (string.IsNullOrEmpty(spawnPointID))
            {
                Debug.Log("This bitch empty.  Yeet!");
            }

            // Find the spawn point in the scene
            Transform spawnPoint = GameObject.Find(spawnPointID)?.transform;

            if (spawnPoint != null)
            {
                // Move the player to the spawn point
                transform.position = spawnPoint.position;
                Debug.Log("Teleported to spawn point: " + spawnPointID);
            }
            else
            {
                Debug.LogWarning($"Spawn point with ID '{spawnPointID}' not found in the scene.");
            }
            //Clear after use
            ArticyGlobalVariables.Default.GlobalVariables.SpawnPoint = "";
            spawnPointID = "";
        }

        private void Update()
        {
            inDialogue = dialogueManager.GetComponent<DialogueManager>().DialogueActive;

            if (isMounted)
                _animator.SetBool("Mounted", true);
            else
                _animator.SetBool("Mounted", false);

            if (isFalling)
                _animator.SetBool("FallingFromHorse", true);
            else
                _animator.SetBool("FallingFromHorse", false);

            if (tempInteractableObject != null)
                interactionRange = tempInteractableObject.GetComponent<Interactable>().interactionRange;
            _hasAnimator = TryGetComponent(out _animator);

            HandleClickMovement(); // New function for handling click movement
            JumpAndGravity();
            GroundedCheck();
            if (!isMounted)
                Move();
            Pause();

            // Check if the Investigate key is being held down
            /*if (_input.investigate) // Assuming "investigate" is the action name in your input system
            {
                hasThought = true;
            }
            else
            {
                hasThought = false;
            }*/

            if (tempInteractableObject != null)
            {
                float distance = Vector3.Distance(transform.position, tempInteractableObject.transform.position);

                if (distance <= interactionRange && tempInteractableObject.name != "Horse") // Ensure interactionRange is defined
                {
                    _isMovingToClick = false;
                    _isSprintingToClick = false;
                    tempInteractableObject.GetComponent<Interactable>().OnInteract();
                    tempInteractableObject = null; // Clear after interaction
                }
            }

            if (paused)
            {
                if (!inMenu)
                    pauseMenu.SetActive(true);
                HUD.SetActive(false);
                Time.timeScale = 0f;
            }
            else if (!paused)
            {
                pauseMenu.SetActive(false);
                HUD.SetActive(true);
                Time.timeScale = 1.0f;
            }

            if (hasThought || Input.GetKey(KeyCode.Tab))
            {
                thoughtCircle.SetActive(true);
                if (tempThoughtTrigger != null)
                {
                    reptilianThoughtBubble.SetActive(tempThoughtTrigger.GetComponent<ThoughtTrigger>().reptilianThought);
                    paleomammalianThoughtBubble.SetActive(tempThoughtTrigger.GetComponent<ThoughtTrigger>().paleomammalianThought);
                    neomammalianThoughtBubble.SetActive(tempThoughtTrigger.GetComponent<ThoughtTrigger>().neomammalianThought);
                    paleThoughtBubble.SetActive(tempThoughtTrigger.GetComponent<ThoughtTrigger>().paleThought);

                    reptilianThoughtBubble.GetComponent<ThoughtInteractionManager>().thoughtTrigger = tempThoughtTrigger;
                    paleomammalianThoughtBubble.GetComponent<ThoughtInteractionManager>().thoughtTrigger = tempThoughtTrigger;
                    neomammalianThoughtBubble.GetComponent<ThoughtInteractionManager>().thoughtTrigger = tempThoughtTrigger;
                    paleThoughtBubble.GetComponent<ThoughtInteractionManager>().thoughtTrigger = tempThoughtTrigger;
                }
            }
            else
            {
                thoughtCircle.SetActive(false);
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        public void Move()
        {
            // set target speed based on move speed, sprint speed, and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // If no movement input, set target speed to 0
            if (!_isMovingToClick && _input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            if (_isMovingToClick && _input.move != Vector2.zero)
            {
                _isMovingToClick = false;
                _isSprintingToClick = false;
            }

            // Current horizontal speed
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // Accelerate/decelerate smoothly
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f; // Round to 3 decimal places
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // Normalize input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // Apply gravity independently of movement
            if (!_controller.isGrounded)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
            else
            {
                _verticalVelocity = 0f; // Reset when on the ground
            }

            Vector3 moveDirection = new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime; // Always apply vertical movement

            // Movement when clicking to move
            if (_isMovingToClick)
            {
                targetSpeed = _isSprintingToClick ? SprintSpeed : MoveSpeed;

                Vector3 direction = (_targetPosition - transform.position).normalized;
                direction.y = 0;

                if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
                {
                    moveDirection += direction * targetSpeed * Time.deltaTime;

                    // Smooth rotation
                    float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), RotationSmoothTime);

                    _animator.SetFloat(_animIDSpeed, targetSpeed);
                    _animator.SetFloat(_animIDMotionSpeed, 1f);
                }
                else
                {
                    _isMovingToClick = false;
                    _isSprintingToClick = false;
                }
            }
            // Movement using WASD
            else if (_input.move != Vector2.zero && !paused && !inDialogue && !isMounted && movementEnabled)
            {
                tempInteractableObject = null;
                targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // Rotate player
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
                moveDirection += targetDirection.normalized * (_speed * Time.deltaTime);

                if (_hasAnimator)
                {
                    _animator.SetFloat(_animIDSpeed, _animationBlend);
                    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                }
            }
            else
            {
                // Stop animation when not moving
                _animator.SetFloat(_animIDSpeed, 0f);
                _animator.SetFloat(_animIDMotionSpeed, 0f);
            }

            // Apply movement (both horizontal and vertical)
            _controller.Move(moveDirection);
        }


        private void JumpAndGravity()
        {
            if (Grounded && !paused && movementEnabled && !inDialogue)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f && !inDialogue && !isMounted)
                {
                    // Cancel click-to-move if any movement key is pressed
                    if (_isMovingToClick)
                    {
                        _isMovingToClick = false;
                        _isSprintingToClick = false;
                    }
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        //_animator.SetBool(_animIDFreeFall, true);
                        //Temp Fix for freefall animation bug
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void Pause()
        {
            //Open Pause Menu
            //Currently has pause activate right when dialogue ends
            if (_input.pause)
            {
                Pause2();
            }
        }

        public void Pause2()
        {
            paused = !paused;
            _input.pause = false; // Reset input so it doesn't continuously trigger
            if (pauseMenu.activeSelf)
            {
                pauseMenu.GetComponent<PauseMenu>().Intro();
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
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        private void HandleClickMovement()
        {
            if (Input.GetMouseButtonDown(0) && !paused && !inDialogue && !isMounted && movementEnabled) // Left mouse button
            {
                ClickMove();
            }
        }

        //Set the tempInteractableObject to the object the player entered
        private void OnTriggerEnter(Collider other)
        {
            // Check if the object has the "ThoughtTrigger" script
            if (other.GetComponent<ThoughtTrigger>() != null)
            {
                // Set tempThoughtTrigger to the object the player entered
                tempThoughtTrigger = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Check if the object has the "ThoughtTrigger" script
            if (other.GetComponent<ThoughtTrigger>() != null)
            {
                // Reset tempThoughtTrigger when the player exits the trigger
                tempThoughtTrigger = null;
            }
        }
        
        public void ClickMove()
        {
            float timeSinceLastClick = Time.time - _lastClickTime;

                if (timeSinceLastClick <= _doubleClickThreshold)
                {
                    // Double click detected
                    _isSprintingToClick = true;
                }
                else
                {
                    _isSprintingToClick = false;
                }

                _lastClickTime = Time.time;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayers)) // Ensure we hit the ground
                {
                    if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Interactable"))
                    {
                        if (!hit.collider.CompareTag("Interactable"))
                            tempInteractableObject = null;
                        _targetPosition = hit.point;
                        _isMovingToClick = true;

                        // Spawn click indicator with correct rotation
                        GameObject indicator = Instantiate(clickIndicatorPrefab, hit.point + Vector3.up * 0.01f, Quaternion.Euler(90, 0, 0));
                    }
                    else
                    {
                        _isMovingToClick = false; // Ignore movement if not on Ground
                    }
                }
        }

        public void MoveToInteractable()
        {
            if (tempInteractableObject != null)
            {
                _targetPosition = tempInteractableObject.transform.position;
                _isMovingToClick = true;
                _isSprintingToClick = false;
            }
        }

        public void MoveToClick(Vector3 position)
        {
            _targetPosition = position;
            _isMovingToClick = true;
            _isSprintingToClick = false;
        }
        //Set thought trigger

        public void Dismount()
        {
            isMounted = false;
        }
    }
}