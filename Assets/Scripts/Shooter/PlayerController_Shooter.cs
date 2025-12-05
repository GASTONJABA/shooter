using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Shooter : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Camera")]
    public float mouseSensitivity = 2f;
    public Camera playerCamera;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction scrollAction;   // ← NUEVA ACCIÓN PARA LA RUEDA

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isGrounded;
    private float verticalLookRotation = 0f;

    private PlayerShooting ShootController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        ShootController = GetComponent<PlayerShooting>();
        ShootController.playerCamera = playerCamera;

        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        scrollAction = playerInput.actions["Scroll"];
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        lookAction.performed += OnLook;
        lookAction.canceled += OnLook;

        jumpAction.performed += OnJump;

        shootAction.performed += OnShoot;

        scrollAction.performed += OnScroll; 
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        lookAction.performed -= OnLook;
        lookAction.canceled -= OnLook;

        jumpAction.performed -= OnJump;

        shootAction.performed -= OnShoot;

        scrollAction.performed -= OnScroll;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    // ---- MOVIMIENTO ----
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

    // ---- CAMARA ----
    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        playerCamera.transform.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
    }

    // ---- SALTO ----
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump!");
        }
    }

    // ---- DISPARO ----
    private void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootController.OnShoot(context);
        }
    }

    // ---- CAMBIO DE ARMA CON RUEDA ----
    private void OnScroll(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<Vector2>().y;

        if (value > 0f)
        {
            ShootController.shootProjectile = true;
            Debug.Log("Weapon Mode: Projectile");
        }
        else if (value < 0f)
        {
            ShootController.shootProjectile = false;
            Debug.Log("Weapon Mode: Raycast");
        }
    }

    // ---- DETECCIÓN SUELO ----
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
