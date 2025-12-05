using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Platformer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    private float coyoteTimer = 0f;

    //[Header("Camera")]
    //public float mouseSensitivity = 2f;
    //public Camera playerCamera;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private Vector2 moveInput;
   // private Vector2 lookInput;
    private bool isGrounded;
   // private float verticalLookRotation = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

       // lookAction.performed += OnLook;
       // lookAction.canceled += OnLook;

        jumpAction.performed += OnJump;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

       // lookAction.performed -= OnLook;
       // lookAction.canceled -= OnLook;

        jumpAction.performed -= OnJump;
    }

    private void Update()
    {
        HandleMovement();
        //HandleMouseLook();
        UpdateCoyoteTimer();
    }

    // --- COYOTE TIMER ---
    private void UpdateCoyoteTimer()
    {
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    // --- MOVIMIENTO ---
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        Vector3 velocity = new Vector3(
            move.x * moveSpeed,
            rb.velocity.y,
            move.z * moveSpeed
        );

        rb.velocity = velocity;
    }

    // --- CAMARA ---
    //private void OnLook(InputAction.CallbackContext context)
    //{
    //    lookInput = context.ReadValue<Vector2>();
    //}

    //private void HandleMouseLook()
    //{
    //    float mouseX = lookInput.x * mouseSensitivity;
    //    float mouseY = lookInput.y * mouseSensitivity;

    //    transform.Rotate(Vector3.up * mouseX);

    //    verticalLookRotation -= mouseY;
    //    verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

    //    playerCamera.transform.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
    //}

    // --- SALTO CON COYOTE TIME ---
    private void OnJump(InputAction.CallbackContext context)
    {
        if (coyoteTimer > 0f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump!");
            coyoteTimer = 0f;
        }
    }

    // --- DETECCIÓN SUELO ---
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}