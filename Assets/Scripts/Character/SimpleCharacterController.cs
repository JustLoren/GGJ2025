using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCharacterController : MonoBehaviour
{
    [Header("Input Action (Vector2)")]
    // Drag & drop your Input Action from the inspector here
    public InputActionReference movementAction;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Enable the action so we can read values from it
        if (movementAction != null)
        {
            movementAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable the action when this script isn't active
        if (movementAction != null)
        {
            movementAction.action.Disable();
        }
    }

    private void Update()
    {
        // Read the movement input (Vector2) from the new Input System
        if (movementAction != null)
        {
            moveInput = movementAction.action.ReadValue<Vector2>();
        }
    }

    private void FixedUpdate()
    {
        var newVelocity = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
        // Move the character with physics
        rb.linearVelocity = newVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Simple collision detection
        Debug.Log($"Collided with {collision.gameObject.name}");
    }
}
