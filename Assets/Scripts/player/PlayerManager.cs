using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class PlayerManager : MonoBehaviourPun, IPunObservable
{
    public static PlayerManager LocalInstance;

    [HideInInspector]
    public int era = -1;

    [HideInInspector]
    public string playerName;

    public Animator animator;
    public CharacterController controller;
    public SkinnedMeshRenderer avatar;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float mouseSensitivity = 100f;
    public float cursorReach = 10f;

    public Camera firstPersonCamera;
    public GameObject menu;
    public GameObject overlay;
    public PlayerInfo playerInfo;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Recorder photonRecorder;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool inMenu = false;

    private bool InLobby { get { return GameLobbyBrain.Instance != null; } }
    private bool InEscapeRoom { get { return GameManager.Instance != null; } }

    private void Awake()
    {

        bool mine = IsMine();

        firstPersonCamera.enabled = mine;

        avatar.shadowCastingMode = mine
            ? UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly
            : UnityEngine.Rendering.ShadowCastingMode.On;

        // Tag local player instance to not destroy when loadong various scenes.
        if (mine)
        {
            PlayerManager.LocalInstance = this;
            Cursor.lockState = CursorLockMode.Locked;
            playerName = PhotonNetwork.NickName;
            playerInfo.enabled = false;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ignoreCollisions();
    }

    private void ignoreCollisions()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");

        Collider collider = GetComponent<Collider>();

        foreach (GameObject otherGameObject in gameObjects)
        {

            if (otherGameObject != this.gameObject)
            {
                Collider otherCollider = otherGameObject.GetComponent<Collider>();
                Physics.IgnoreCollision(collider, otherCollider);
            }
        }
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
                UpdateCursor();
                //UpdateOverlay();

                if (selectedEntity != null && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("GRAB");
                    animator.SetTrigger("Grab");
                    selectedEntity.Interact(this);
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    photonRecorder.TransmitEnabled = true;
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    photonRecorder.TransmitEnabled = false;
                }
            }

            if (InLobby)
            {
                era = GameLobbyBrain.Instance.EraForPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }

        playerInfo.UpdateInfo(playerName, era);
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
                CloseMenu();
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                OpenMenu();
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
            velocity.y = -0.5f;
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

        Debug.LogFormat("Animator parameters: {0} {1} {2}", isGrounded, move.sqrMagnitude > 0f, velocity.magnitude);

        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("IsWalking", move.sqrMagnitude > 0f);
        animator.SetFloat("VerticalSpeed", velocity.magnitude);
    }

    private Entity selectedEntity;
    private void UpdateCursor()
    {
        //Debug.Log("casting ray");
        //Debug.DrawRay(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, Color.green);

        RaycastHit hit;
        Ray ray = new Ray(firstPersonCamera.transform.position, firstPersonCamera.transform.forward);

        if (Physics.Raycast(ray, out hit, cursorReach, LayerMask.GetMask("Entities")))
        {
            Entity entity = hit.transform.GetComponent<Entity>();

            if (selectedEntity != null && selectedEntity != entity)
            {
                selectedEntity.Highlighted = false;
            }

            if (entity != null)
            {
                selectedEntity = entity;
                selectedEntity.Highlighted = true;
            }
        }
        else if (selectedEntity != null)
        {
            selectedEntity.Highlighted = false;
            selectedEntity = null;
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OpenMenu()
    {
        inMenu = true;
        menu.SetActive(true);
        overlay.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu()
    {
        inMenu = false;
        menu.SetActive(false);
        overlay.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerName);

            stream.SendNext(era);
        }
        else
        {
            this.playerName = (string)stream.ReceiveNext();

            this.era = (int)stream.ReceiveNext();
        }
    }
}
