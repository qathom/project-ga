using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class PlayerManager : MonoBehaviourPun
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public Animator animator;
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float mouseSensitivity = 100f;

    public Camera firstPersonCamera;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Recorder photonRecorder;

    private float xRotation = 0f;
    public Vector3 velocity;
    public double magnitude;
    private bool isGrounded;
    private bool inMenu = false;

    private void Awake()
    {
        // Tag local player instance to not destroy when loadong various scenes.
        if (IsMine())
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;

            Cursor.lockState = CursorLockMode.Locked;
        }

        DontDestroyOnLoad(this.gameObject);

        
        firstPersonCamera.enabled = IsMine();
    }

    private void Start()
    {
        //photonRecorder.TransmitEnabled = IsMine();
    }

    private void Update()
    {
        if (IsMine())
        {
            UpdateMenu();

            if (!inMenu)
            {
                UpdateCamera();
                UpdateMovement();

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    photonRecorder.TransmitEnabled = true;
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    photonRecorder.TransmitEnabled = false;
                }
            }
        }
    }

    private bool IsMine()
    {
        return photonView.IsMine || !PhotonNetwork.IsConnected;
    }

    private void UpdateMenu()
    {
        if (inMenu)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                inMenu = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                inMenu = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void UpdateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        firstPersonCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

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

        magnitude = move.magnitude;
        if (move.magnitude > 0.1f)
        {
            animator.Play("walk");
        } else
        {
            animator.Play("idle");
        }

        if (Input.GetKeyDown("e"))
        {
            animator.Play("grab");
        }
    }
}
