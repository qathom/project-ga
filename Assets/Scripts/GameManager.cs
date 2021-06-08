using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using System;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField]
    private int playerCount = 3;
    private int escapedPlayerCount = 0;

    private bool gameEnded = false;
    private float leaveCooldown = 5f;

    public bool HasGameEnded { get { return gameEnded; } }
    public float LeaveCooldown { get { return leaveCooldown; } }

    private void Awake()
    {
        _instance = this;
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    private void Start()
    {
        setRoomEnv(PlayerManager.LocalInstance.era);
        PlayerManager.LocalInstance.Teleport();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!gameEnded && escapedPlayerCount >= playerCount)
            {
                // Game Ended
                gameEnded = true;
            }

            if (gameEnded)
            {
                leaveCooldown -= Time.deltaTime;
                if (leaveCooldown <= 0f)
                {
                    photonView.RPC("GameEnded", RpcTarget.AllBufferedViaServer);
                }
            }
        }
    }

    private void setRoomEnv(int playerEra)
    {
        //print("Setting view in epoque: " + tag);

        foreach (int era in PlayerEra2.Values)
        {
            bool active = playerEra == era;
            GameObject[] gameObjects = GameObject
                .FindGameObjectsWithTag(PlayerEra2.TagForEra(era));

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(active);
            }
        }
    }

    public override void OnLeftRoom()
    {
        if (!gameEnded){
            SceneManager.LoadScene(0);
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        if (!gameEnded)
        {
            Debug.Log("Sombody left. Also leaving.");
            PhotonNetwork.LeaveRoom();
        }
    }


    public void sendPlayerEscapedMessage()
    {
        photonView.RPC("PlayerEscaped", RpcTarget.MasterClient);
    }

    #region Pun RPCs

    [PunRPC]
    public void PlayerEscaped()
    {
        escapedPlayerCount += 1;
    }

    [PunRPC]
    public void GameEnded()
    {
        PhotonNetwork.LeaveRoom();
        DontDestroyOnLoad(GameStatistics.Instance);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(4);
    }

    #endregion

    #region Pun Observable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(escapedPlayerCount);
            stream.SendNext(gameEnded);
            stream.SendNext(leaveCooldown);
        }
        else
        {
            escapedPlayerCount = (int)stream.ReceiveNext();
            gameEnded = (bool)stream.ReceiveNext();
            leaveCooldown = (float)stream.ReceiveNext();
        }
    }

    #endregion
}
