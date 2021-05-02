using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Laucher : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 3;

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    private bool isConnecting;

    private void Awake()
    {
        Debug.Log("Awake");

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
    }

    public void Connect()
    {
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);

        Debug.Log("Connect");

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);

        Debug.LogWarningFormat("Disconnect with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join random room failed. Creating new room...");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");

        // Critical: We only load if we are the first player, else we rely on  PhotonNetwork.automaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Load Room 1");

            PhotonNetwork.LoadLevel("Room 1");
        }
    }

    #endregion
}
