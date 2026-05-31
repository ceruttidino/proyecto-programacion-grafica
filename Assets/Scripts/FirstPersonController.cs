using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCamera;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Mouse")]
    [SerializeField] private float mouseSensitivity = 0.15f;

    private CharacterController controller;
    private PlayerControls controls;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float cameraPitch;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls.Player.Move.performed +=
            ctx => moveInput = ctx.ReadValue<Vector2>();

        controls.Player.Move.canceled +=
            ctx => moveInput = Vector2.zero;

        controls.Player.Look.performed +=
            ctx => lookInput = ctx.ReadValue<Vector2>();

        controls.Player.Look.canceled +=
            ctx => lookInput = Vector2.zero;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleLook()
    {
        float mouseX =
            lookInput.x * mouseSensitivity;

        float mouseY =
            lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;

        cameraPitch =
            Mathf.Clamp(cameraPitch, -80f, 80f);

        playerCamera.localRotation =
            Quaternion.Euler(cameraPitch, 0, 0);
    }

    void HandleMovement()
    {
        Vector3 direction =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        controller.Move(
            direction *
            moveSpeed *
            Time.deltaTime);
    }
}