using UnityEngine; // Brings in Unity's core classes (MonoBehaviour, Vector3, Input, etc.)

[RequireComponent(typeof(CharacterController))] // Forces this GameObject to always have a CharacterController component attached
public class PlayerMovement : MonoBehaviour // Declares this script as a component that can be attached to a GameObject
{
    public float walkSpeed = 6f;   // Walking speed, editable in the Inspector
    public float jumpPower = 7f;   // Upward force applied when jumping, editable in the Inspector
    public float gravity = 10f;    // How strongly the player is pulled down when airborne, editable in the Inspector

    private Vector3 moveDirection = Vector3.zero; // Stores the current movement vector (x = sideways, y = vertical/jump, z = forward/back)
    private CharacterController characterController; // Reference to the CharacterController component that actually moves the object

    private bool canMove = true; // Flag that allows movement to be enabled/disabled (kept in case you want to lock movement later, e.g. during a cutscene)

    void Start() 
    {
        characterController = GetComponent<CharacterController>(); // Grabs the CharacterController attached to this same GameObject and stores it for later use
    }

    void Update() 
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward); // Converts the object's local "forward" direction into world-space, so movement follows where the player is facing
        Vector3 right = transform.TransformDirection(Vector3.right);     // Converts the object's local "right" direction into world-space, same reason as above

        float curSpeedX = canMove ? walkSpeed * Input.GetAxis("Vertical") : 0;
        // ^ Calculates forward/back speed: walkSpeed multiplied by the Vertical input axis
        //   (W/S or Up/Down, ranges -1 to 1), or 0 if movement is disabled

        float curSpeedY = canMove ? walkSpeed * Input.GetAxis("Horizontal") : 0;
        // ^ Same idea as above but for sideways speed, using the Horizontal input axis (A/D or Left/Right)

        float movementDirectionY = moveDirection.y; // Temporarily saves the current vertical speed (important so jumping/falling isn't overwritten below)

        moveDirection = (forward * curSpeedX) + (right * curSpeedY); // Combines forward and sideways movement into one final direction vector

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded) // If the Jump button is pressed, movement is allowed, AND the player is touching the ground
        {
            moveDirection.y = jumpPower; // Sets vertical speed to the jump power, launching the player upward
        }
        else // If not jumping this frame
        {
            moveDirection.y = movementDirectionY; // Restores the vertical speed from before (keeps falling/jump arc consistent instead of resetting it)
        }

        if (!characterController.isGrounded) // If the player is currently in the air (not touching ground)
        {
            moveDirection.y -= gravity * Time.deltaTime; // Applies gravity over time, pulling the player back down
        }

        characterController.Move(moveDirection * Time.deltaTime); // Actually moves the CharacterController by the calculated direction, scaled by frame time so speed is consistent across framerates
    }
}