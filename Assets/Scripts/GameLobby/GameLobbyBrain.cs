using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameLobbyBrain : MonoBehaviourPunCallbacks, IPunObservable
{
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private int playerCount = 1;

    private static GameLobbyBrain _instance;
    public static GameLobbyBrain Instance { get { return _instance; } }

    private Dictionary<int, int> playerEras = new Dictionary<int, int>();

    public bool IsReady { get { return playerEras.Count == playerCount; } }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.LocalInstance == null)
        {
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerEras);
        }
        else
        {
            playerEras = (Dictionary<int, int>)stream.ReceiveNext();
        }
    }

    public int PlayerForEra(int era)
    {
        if (playerEras.ContainsKey(era))
        {
            return playerEras[era];
        }

        return -1;
    }

    public int EraForPlayer(int player)
    {
        foreach (int era in playerEras.Keys)
        {
            if (playerEras[era] == player)
            {
                return era;
            }
        }

        return -1;
    }

    [PunRPC]
    public void PlayerJoinOrLeaveEra(int era, int player, bool join)
    {
        if (join)
        {
            if (PlayerForEra(era) < 0 && EraForPlayer(player) < 0)
            {
                playerEras.Add(era, player);
            }
        }
        else
        {
            if (PlayerForEra(era) == player)
            {
                playerEras.Remove(era);
            }
        }
    }

    #region Mono Behavior Pun Callbacks

    public override void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.LogFormat("left {0}", other.ActorNumber);

            int era = GameLobbyBrain.Instance.EraForPlayer(other.ActorNumber);
            if (era >= 0)
            {
                Debug.Log("leave era");
                GameLobbyBrain.Instance.photonView.RPC(
                    "PlayerJoinOrLeaveEra", RpcTarget.MasterClient,
                    era, other.ActorNumber, false);
            }
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion
}