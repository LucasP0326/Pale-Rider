using StarterAssets;
using UnityEngine;

public class HorseManager : MonoBehaviour
{
    private Transform player;
    public Transform playerMountPosition;
    public GameObject playerController;
    
    [Header("Horse Movement")]
    public float walkSpeed = 2.0f;
    public float trotSpeed = 4.0f;
    public float gallopSpeed = 10.0f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;
    public float jumpCooldown = 0.5f;
    public bool canJump = true;
    
    [Header("Speed Control")]
    public float speedIncreasePerTap = 0.5f; // How much speed increases per shift tap
    public float speedDecayRate = 0.2f; // How fast speed decays per second
    public float minimumSpeed = 0.0f; // Minimum speed (idle)
    private float targetSpeed; // The speed we're trying to reach
    private float currentSpeed;
    private bool wasSprintPressed; // To detect shift key press

    [Header("Ground Check")]
    public float groundCheckDistance = 0.1f; // Reduced from 0.5f to 0.1f for more precise ground detection
    public float groundedOffset = -0.14f; // Added to fine-tune ground detection position
    public LayerMask groundLayer;
    private bool isGrounded;

    private CharacterController horseController;
    private StarterAssetsInputs playerInput;
    private Vector3 moveDirection;
    private float verticalVelocity;
    private readonly float gravity = -9.81f;
    private Vector3 clickTarget;
    private bool isMovingToClick;

    [Header("Animator")]
    public Animator horseAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.FindGameObjectWithTag("Player");
        horseController = GetComponent<CharacterController>();
        if (horseController == null)
        {
            horseController = gameObject.AddComponent<CharacterController>();
        }
        
        // Configure the CharacterController to match the horse's actual size
        horseController.height = 1.6f; // Adjust this to match your horse's height
        horseController.radius = 0.4f; // Adjust this to match your horse's width
        horseController.center = new Vector3(0, horseController.height / 2, 0); // Center the controller on the horse
        horseController.stepOffset = 0.1f; // Small step offset for smooth ground movement
        
        horseController.enabled = false; // Start disabled until mounted
        targetSpeed = minimumSpeed;
        currentSpeed = minimumSpeed;

        horseAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.GetComponent<ThirdPersonController>().isMounted)
            return;

        CheckGround();
        HandleMovement();
        HandleJump();

        horseAnimator.SetFloat("Speed", currentSpeed);
        if (currentSpeed <= 0.1f)
        {
            horseAnimator.SetBool("IsIdling", true);
        }
        else
        {
            horseAnimator.SetBool("IsIdling", false);
        }
        //Debug.Log("Horse Speed: " + currentSpeed);
    }

    private void CheckGround()
    {
        // Origin point is at the bottom of the character controller
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundCheckDistance, groundLayer);
    }

    private void HandleMovement()
    {
        playerInput = playerController.GetComponent<StarterAssetsInputs>();
        
        // Get input direction
        Vector3 inputDirection = new Vector3(playerInput.move.x, 0.0f, playerInput.move.y);

        // Cancel click-to-move if player uses WASD
        if (inputDirection.magnitude >= 0.1f)
        {
            isMovingToClick = false;
        }

        // Handle click-to-move
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                clickTarget = hit.point;
                isMovingToClick = true;
            }
        }

        // Calculate movement direction
        if (isMovingToClick)
        {
            Vector3 directionToTarget = (clickTarget - transform.position).normalized;
            directionToTarget.y = 0;
            if (Vector3.Distance(transform.position, clickTarget) < 0.5f)
            {
                isMovingToClick = false;
                inputDirection = Vector3.zero;
            }
            else
            {
                inputDirection = transform.InverseTransformDirection(directionToTarget);
            }
        }

        // Determine whether the horse should be moving (walk) or idle
        bool isMoving = inputDirection.magnitude >= 0.1f || isMovingToClick;

        // Handle speed increase on Shift tap (detect taps)
        if (playerInput.sprint && !wasSprintPressed)
        {
            // Increase target speed but cap it at gallop speed
            targetSpeed = Mathf.Min(targetSpeed + speedIncreasePerTap, gallopSpeed);
        }
        wasSprintPressed = playerInput.sprint;

        // If moving, ensure target is at least walk speed; if idle, target is minimum (idle = 0)
        if (isMoving)
        {
            if (targetSpeed < walkSpeed)
                targetSpeed = walkSpeed;

            // If not sprinting, decay speed back toward walk speed
            if (!playerInput.sprint && targetSpeed > walkSpeed)
            {
                targetSpeed -= speedDecayRate * Time.deltaTime;
                targetSpeed = Mathf.Max(targetSpeed, walkSpeed);
            }
        }
        else
        {
            // Not moving: decay toward idle minimum
            if (targetSpeed > minimumSpeed)
            {
                targetSpeed -= speedDecayRate * Time.deltaTime;
                targetSpeed = Mathf.Max(targetSpeed, minimumSpeed);
            }
        }

        // Smoothly interpolate current speed to target speed
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 5f);

        // Apply movement
        if (isMoving)
        {
            // Get the camera's forward direction, ignoring pitch
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // Calculate the target rotation based on input and camera
            Quaternion targetRotation;
            if (isMovingToClick)
            {
                targetRotation = Quaternion.LookRotation(clickTarget - transform.position);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(
                    Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * inputDirection
                );
            }

            // Smoothly rotate towards the target direction
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Move in the facing direction using the current speed
            moveDirection = transform.forward * currentSpeed;
        }
        else
        {
            moveDirection = Vector3.zero;
            // Force target to idle and snap current speed to zero so animator stays idle
            targetSpeed = minimumSpeed;
            currentSpeed = 0f;
        }

        // Apply gravity
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        moveDirection.y = verticalVelocity;

        // Move the horse
        horseController.Move(moveDirection * Time.deltaTime);

        // Update player position to mount position
        player.position = playerMountPosition.position;
        player.rotation = playerMountPosition.rotation;
    }

    private void HandleJump()
    {
        if (!canJump || !isGrounded) return;

        if (playerInput.jump)
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
            canJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    public void Mount()
    {
        var tpc = playerController.GetComponent<ThirdPersonController>();
        tpc.isMounted = true;
        
        // Disable player's CharacterController
        var playerCharController = playerController.GetComponent<CharacterController>();
        if (playerCharController != null)
            playerCharController.enabled = false;

        // Enable horse's CharacterController
        horseController.enabled = true;

        // Move player to mount position
        player.position = playerMountPosition.position;
        player.rotation = playerMountPosition.rotation;
        player.parent = transform; // Parent player to horse

        // Ensure speeds are reset when mounting so the horse starts idle
        targetSpeed = minimumSpeed;
        currentSpeed = minimumSpeed;
        wasSprintPressed = false;
    }

    public void Dismount()
    {
        var tpc = playerController.GetComponent<ThirdPersonController>();
        tpc.isMounted = false;

        // Re-enable player's CharacterController
        var playerCharController = playerController.GetComponent<CharacterController>();
        if (playerCharController != null)
            playerCharController.enabled = true;

        // Disable horse's CharacterController
        horseController.enabled = false;

        // Unparent and position player slightly to the side of the horse
        player.parent = null;
        Vector3 dismountPosition = transform.position + transform.right * 2f;
        player.position = dismountPosition;

        // Reset speeds to idle so animator doesn't keep walking after dismount
        targetSpeed = minimumSpeed;
        currentSpeed = minimumSpeed;
        wasSprintPressed = false;
    }
}
