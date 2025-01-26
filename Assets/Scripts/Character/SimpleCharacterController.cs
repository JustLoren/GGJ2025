using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCharacterController : MonoBehaviour
{
    private static SimpleCharacterController _instance;
    private void Start()
    {
        if (_instance == null)
            _instance = this;
    }

    private void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    public static void AllowMovement(bool allow)
    {
        _instance.allowMovement = allow;
    }

    private bool allowMovement = true;

    [Header("Input Action (Vector2)")]
    // Drag & drop your Input Action from the inspector here
    public InputActionReference movementAction;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;
    public BouncySprite bouncer;

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
        //No movement allowed, zero this shit out
        if (!allowMovement)
        {
            moveInput = Vector2.zero;
            bouncer.SetMoving(moveInput * moveSpeed);
            return;
        }

        // Read the movement input (Vector2) from the new Input System
        if (movementAction != null)
        {
            moveInput = movementAction.action.ReadValue<Vector2>();
            bouncer.SetMoving(moveInput * moveSpeed);
        }
    }

    private void EnsureCamera()
    {
        if (cameraMain == null)
            cameraMain = Camera.main.transform;
    }

    private Transform cameraMain;
    private void FixedUpdate()
    {
        EnsureCamera();

        var newVelocity = new Vector3(moveInput.x, 0f, moveInput.y);
        var m_CamForward = Vector3.Scale(cameraMain.forward, new Vector3(1, 0, 1)).normalized;
        newVelocity = newVelocity.z * m_CamForward + newVelocity.x * cameraMain.right;
        newVelocity.y = 0f;
        newVelocity = newVelocity * moveSpeed;

        // Move the character with physics
        rb.linearVelocity = newVelocity;
        if (newVelocity.magnitude > 0)
        {
            rb.MoveRotation(Quaternion.LookRotation(newVelocity));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Simple collision detection
        Debug.Log($"Collided with {collision.gameObject.name}");
    }
}
