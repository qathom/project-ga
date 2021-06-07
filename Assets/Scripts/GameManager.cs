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

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        PlayerManager.LocalInstance.transform.position = new Vector3(0, 1, 0);
        setRoomEnv(PlayerManager.LocalInstance.era);
    }

    private void Update()
    {
        if (!gameEnded)
        {
            if (escapedPlayerCount >= playerCount)
            {
                // TODO: load end game scene
                gameEnded = true;
            }
        }

        if (gameEnded)
        {
            print("GAME ENDED");

            // TODO: save local data & load quizz scene
        }  
    }

    private void setRoomEnv(int playerEra)
    {
        print("Setting view in epoque: " + tag);

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
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("Sombody left. Also leaving.");
        PhotonNetwork.LeaveRoom();
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

    #endregion

    #region Pun Observable

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameEnded);
        }
        else
        {
            gameEnded = (bool)stream.ReceiveNext();
        }
    }

    #endregion
}
