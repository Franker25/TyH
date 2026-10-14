using UnityEngine; // Brings in Unity's core classes (MonoBehaviour, Vector3, Transform, etc.)

[RequireComponent(typeof(CharacterController))] // Forces this GameObject to always have a CharacterController component attached
[RequireComponent(typeof(SphereCollider))]       // Forces this GameObject to always have a SphereCollider component attached (used as the detection range)
public class NPCChase : MonoBehaviour // Declares this script as a component that can be attached to the NPC GameObject
{
    public Transform player;       // Reference to the Player's Transform, so the NPC knows where to walk toward. Drag the Player object here in the Inspector
    public float walkSpeed = 6f;   // Speed at which the NPC chases the player, editable in the Inspector
    public float gravity = 10f;    // How strongly the NPC is pulled down when airborne, editable in the Inspector
    public float detectionRadius = 10f; // How close the player must be (in meters) before the NPC starts chasing, editable in the Inspector

    private Vector3 moveDirection = Vector3.zero; // Stores the NPC's current movement vector (x = sideways, y = vertical/gravity, z = forward/back)
    private CharacterController characterController; // Reference to the CharacterController component that actually moves the NPC
    private SphereCollider detectionSphere; // Reference to the SphereCollider used purely as a trigger zone for detecting the player

    private bool isChasing = false; // Tracks whether the player is currently inside the detection sphere

    void Start() // Called once when the object is created/enabled
    {
        characterController = GetComponent<CharacterController>(); // Grabs the CharacterController attached to this same GameObject and stores it for later use

        detectionSphere = GetComponent<SphereCollider>(); // Grabs the SphereCollider attached to this same GameObject and stores it for later use
        detectionSphere.isTrigger = true;                 // Forces the sphere to act as a trigger (no physical collision, just detects overlap)
        detectionSphere.radius = detectionRadius;          // Sets the sphere's radius to match the detectionRadius value above, so both stay in sync
    }

    void Update() // Called once every frame
    {
        Vector3 horizontalMove = Vector3.zero; // Starts with no horizontal movement this frame; will be filled in below only if chasing

        if (isChasing && player != null) // Only move toward the player if he's inside the detection sphere and the reference is assigned
        {
            Vector3 direction = player.position - transform.position; // Vector pointing from the NPC to the Player
            direction.y = 0f; // Flattens the direction so the NPC doesn't tilt up/down while chasing, only rotates/moves on the ground plane

            if (direction.sqrMagnitude > 0.01f) // Avoids errors/jitter from normalizing a near-zero vector when the NPC is basically on top of the player
            {
                direction.Normalize(); // Reduces the vector to length 1, keeping only its direction
                horizontalMove = direction * walkSpeed; // Scales the direction by the NPC's walk speed to get the actual movement this frame
                transform.rotation = Quaternion.LookRotation(direction); // Rotates the NPC to visually face the direction it's moving/the player
            }
        }

        moveDirection.x = horizontalMove.x; // Applies the calculated sideways movement to the NPC's move vector
        moveDirection.z = horizontalMove.z; // Applies the calculated forward/back movement to the NPC's move vector

        if (!characterController.isGrounded) // If the NPC is currently in the air (not touching ground)
        {
            moveDirection.y -= gravity * Time.deltaTime; // Applies gravity over time, pulling the NPC back down
        }
        else // If the NPC is on the ground
        {
            moveDirection.y = -0.5f; // Keeps a small constant downward force so isGrounded stays reliably true on slopes/flat ground
        }

        characterController.Move(moveDirection * Time.deltaTime); // Actually moves the CharacterController by the calculated direction, scaled by frame time
    }

    void OnTriggerEnter(Collider other) // Called automatically when something enters the SphereCollider trigger zone
    {
        if (other.CompareTag("Player")) // Checks that it was specifically the Player that entered (Player GameObject must be tagged "Player")
        {
            isChasing = true; // Starts the chase
        }
    }

    void OnTriggerExit(Collider other) // Called automatically when something leaves the SphereCollider trigger zone
    {
        if (other.CompareTag("Player")) // Checks that it was specifically the Player that left
        {
            isChasing = false; // Stops the chase
        }
    }
}
