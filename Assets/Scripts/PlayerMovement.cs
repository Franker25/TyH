using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Automatically adds a CharacterController component to the GameObject if one isn't attached yet.
// Prevents errors caused by a missing CharacterController.
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // --- EXPOSED VARIABLES (Editable in the Unity Inspector) ---

    [Header("Camera & Looking")]
    public Camera playerCamera;      // Reference to the main camera attached to the player (used for vertical looking)
    public float lookSpeed = 2f;      // Sensitivity of the mouse movement
    public float lookXLimit = 45f;    // Maximum angle (up/down) the player can look to prevent flipping the camera

    [Header("Movement Speeds")]
    public float walkSpeed = 6f;      // Normal movement speed
    public float runSpeed = 12f;      // Speed when holding the sprint key
    public float crouchSpeed = 3f;   // Speed when holding the crouch key

    [Header("Jumping & Gravity")]
    public float jumpPower = 7f;      // Upward force applied when jumping
    public float gravity = 10f;       // Downward force applied continuously to simulate gravity

    [Header("Crouching Settings")]
    public float defaultHeight = 2f;  // Normal standing height of the CharacterController collision capsule
    public float crouchHeight = 1f;   // Lowered height of the collision capsule while crouching

    // --- PRIVATE VARIABLES (Internal script state) ---

    private Vector3 moveDirection = Vector3.zero;     // Stores the player's calculated movement vector (X, Y, Z)
    private float rotationX = 0;                      // Tracks current vertical camera angle (pitch)
    private CharacterController characterController;  // Cached reference to the attached CharacterController component
    private bool canMove = true;                      // Master toggle to enable or disable player control

    // Start is called automatically on the frame when the script is enabled
    void Start()
    {
        // Fetch and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();

        // Lock the mouse cursor to the center of the game window and hide it for first-person control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // -------------------------------------------------------------------
        // 1. DIRECTION VECTORS
        // Converts local directional directions (Forward/Right) into global world space vectors
        // -------------------------------------------------------------------
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // -------------------------------------------------------------------
        // 2. SPEED & MOVEMENT CALCULATIONS
        // -------------------------------------------------------------------
        // Check if the player is holding Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Calculate forward/backward speed based on Vertical axis input (W/S or Up/Down arrows)
        // Uses ternary operators: if canMove is true -> check if running -> select speed -> multiply by input direction
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;

        // Calculate left/right speed based on Horizontal axis input (A/D or Left/Right arrows)
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        // Preserve current vertical velocity (gravity/jumping) before recalculating horizontal movement
        float movementDirectionY = moveDirection.y;

        // Combine forward/backward and side-to-side movements into a single horizontal direction vector
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // -------------------------------------------------------------------
        // 3. JUMPING
        // -------------------------------------------------------------------
        // Allow jumping only if the Jump button (Spacebar) is pressed, movement is enabled, and the player touches the ground
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower; // Apply upward jump speed
        }
        else
        {
            moveDirection.y = movementDirectionY; // Maintain current vertical velocity
        }

        // -------------------------------------------------------------------
        // 4. GRAVITY
        // -------------------------------------------------------------------
        // Continuously subtract gravity over time when the character is suspended in the air
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // -------------------------------------------------------------------
        // 5. CROUCHING
        // -------------------------------------------------------------------
        // Check if the 'R' key is held down to crouch
        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight; // Shrink collision capsule
            walkSpeed = crouchSpeed;                  // Slow down walk speed
            runSpeed = crouchSpeed;                   // Disable running speed while crouched
        }
        else
        {
            // Reset character back to default standing parameters when 'R' is released
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        // -------------------------------------------------------------------
        // 6. APPLY MOVEMENT
        // -------------------------------------------------------------------
        // Move the Character Controller using world velocity scaled by frame time (Time.deltaTime ensures smooth movement regardless of framerate)
        characterController.Move(moveDirection * Time.deltaTime);

        // -------------------------------------------------------------------
        // 7. MOUSE LOOK & CAMERA ROTATION
        // -------------------------------------------------------------------
        if (canMove)
        {
            // Vertical Look (Pitch - Camera up/down):
            // Invert Mouse Y input and accumulate to rotationX variable
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;

            // Clamp pitch to avoid full 360-degree flips
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Apply vertical rotation directly to the child Camera object
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Horizontal Look (Yaw - Player body left/right):
            // Rotate the main player object horizontally based on Mouse X input
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}