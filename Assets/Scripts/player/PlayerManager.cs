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

    [SerializeField]
    private Transform hand;

    private float talkDuration = 0.0f;
    private bool talking = false;

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
    public PlayerInfo playerInfo;
    public PlayerOverlay overlay;
    public PlayerInputOverlay inputOverlay;

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

        if (mine)
        {
            print("mine");
            PlayerManager.LocalInstance = this;
            Cursor.lockState = CursorLockMode.Locked;
            playerName = PhotonNetwork.NickName;
            playerInfo.enabled = false;
            CloseMenu();
        }
        else
        {
            print("not mine");
            menu.SetActive(false);
            overlay.SetActive(false);
            inputOverlay.SetActive(false);
        }

        // Tag local player instance to not destroy when loadong various scenes.
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
            UpdateCamera();
            UpdateMovement();
            UpdateCursor();
            UpdateOverlay();

            // Voice
            if (talking)
            {
                talkDuration += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                photonRecorder.TransmitEnabled = true;
                talking = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                photonRecorder.TransmitEnabled = false;
                talking = false;
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

    private void UpdateOverlay()
    {
        if (InEscapeRoom)
        {
            bool gameEnded = GameManager.Instance.HasGameEnded;

            if (inMenu && gameEnded)
            {
                CloseMenu();
            }

            overlay.UpdateGameEnded(gameEnded, GameManager.Instance.LeaveCooldown);
        }

        overlay.UpdateEra(era);

        if (!inMenu)
        {
            if (selectedEntity != null)
            {
                bool canInteract = selectedEntity.CanInteract(this);

                overlay.ToggleSelectionPanel(true);
                overlay.UpdateSelectionPanel(
                    canInteract,
                    selectedEntity.GetInteractionHint(this),
                    selectedEntity.GetDescription(this)
                );

                if (canInteract && Input.GetKeyDown(KeyCode.E))
                {
                    animator.SetTrigger("Grab");
                    selectedEntity.Interact(this);
                }
            }
            else
            {
                overlay.ToggleSelectionPanel(false);
            }
        }
    }

    private void UpdateCamera()
    {
        if (inMenu) return;

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

        bool isWalking = false;
        if (!inMenu)
        {
            // Walking
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
            isWalking = move.sqrMagnitude > 0f;

            //Jump
            if (isGrounded && Input.GetButton("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("IsWalking", isWalking);
        animator.SetFloat("VerticalSpeed", velocity.magnitude);
    }

    public Entity Entity;
    public bool CanPickUpEntity { get { return Entity == null; } }
    private Entity selectedEntity;
    public Entity SelectedEntity { get { return selectedEntity; } }
    private void UpdateCursor()
    {
        if (inMenu) return;

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

    public void PickUpEntity(Entity entity)
    {
        Entity = entity;
        entity.AttachTo(hand);
    }

    public void DropEntity()
    {
        Entity = null;
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
        inputOverlay.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMenu()
    {
        inMenu = false;
        menu.SetActive(false);
        overlay.SetActive(true);
        inputOverlay.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenInputOverlay()
    {
        inMenu = true;
        menu.SetActive(false);
        overlay.SetActive(false);
        inputOverlay.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void SendInput()
    {
        inputOverlay.SendInput();
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerName);

            stream.SendNext(era);

            stream.SendNext(talking);

            stream.SendNext(talkDuration);
        }
        else
        {
            this.playerName = (string)stream.ReceiveNext();

            this.era = (int)stream.ReceiveNext();

            this.talking = (bool)stream.ReceiveNext();

            this.talkDuration = (float)stream.ReceiveNext();
        }

        if (InEscapeRoom && GameStatistics.IsReady)
        {
            GameStatistics.Instance.UpdateTalkDuration(playerName, talkDuration);
        }
    }
}
