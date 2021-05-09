using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    public enum UIIndex
    {
        CONNECT, CONNECTING, ROOMS, LOADING
    }

    [System.Serializable]
    public struct UIPanels
    {
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject connectingLabel;

        [Tooltip("The UI panel containing the list of rooms")]
        [SerializeField]
        private GameObject roomsPanel;

        [Tooltip("The UI panel showing loading")]
        [SerializeField]
        private GameObject loadingPanel;

        public GameObject Get(UIIndex index)
        {
            switch (index)
            {
                case UIIndex.CONNECT:
                    return controlPanel;
                case UIIndex.CONNECTING:
                    return connectingLabel;
                case UIIndex.ROOMS:
                    return roomsPanel;
                case UIIndex.LOADING:
                    return loadingPanel;
                default:
                    return null;
            }
        }

        public void ChangePanel(UIIndex index)
        {
            controlPanel.SetActive(index == UIIndex.CONNECT);
            connectingLabel.SetActive(index == UIIndex.CONNECTING);
            roomsPanel.SetActive(index == UIIndex.ROOMS);
            loadingPanel.SetActive(index == UIIndex.LOADING);
        }
    }

    [SerializeField]
    private UIPanels uiPanels;

    [SerializeField]
    private RoomList roomList;

    private string gameVersion = "1";
    private byte maxPlayersPerRoom = 3;

    private void Awake()
    {
        Debug.Log("awake");

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Debug.Log("start");
        ChangeUI(PhotonNetwork.IsConnected ? UIIndex.ROOMS : UIIndex.CONNECT);
    }

    public void Connect()
    {
        Debug.Log("connect");

        ChangeUI(UIIndex.CONNECTING);

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public void ChangeUI(UIIndex index)
    {
        uiPanels.ChangePanel(index);
    }

    public void CreateRoom()
    {
        ChangeUI(UIIndex.LOADING);

        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public void JoinRoom(RoomInfo room)
    {
        ChangeUI(UIIndex.LOADING);

        PhotonNetwork.JoinRoom(room.Name);
    }

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");

        PhotonNetwork.JoinLobby();

        ChangeUI(UIIndex.ROOMS);
    }

    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        Debug.Log("update room list");
        roomList.UpdateList(rooms, this);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("sisconnect with reason {0}", cause);

        ChangeUI(UIIndex.CONNECT);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogFormat("join random room failed: {0}, {1}", returnCode, message);
        ChangeUI(UIIndex.ROOMS);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogFormat("join room failed: {0}, {1}", returnCode, message);
        ChangeUI(UIIndex.ROOMS);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");

        // Critical: We only load if we are the first player, else we rely on PhotonNetwork.automaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    #endregion
}
