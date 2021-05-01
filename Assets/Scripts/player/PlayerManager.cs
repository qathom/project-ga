using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float mouseSensitivity = 100f;

    public Transform cameraTransform;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        // Tag local player instance to not destroy when loadong various scenes.
        if (IsMine())
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;

            Cursor.lockState = CursorLockMode.Locked;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (IsMine())
        {
            UpdateCamera();
            UpdateMovement();
        }
    }

    private bool IsMine()
    {
        return photonView.IsMine || !PhotonNetwork.IsConnected;
    }

    private void UpdateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void UpdateMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (isGrounded && Input.GetButton("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
