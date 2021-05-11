using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        PlayerManager.LocalInstance.transform.position = new Vector3(0, 1, 0);
        setRoomEnv(PlayerEra2.TagForEra(PlayerManager.LocalInstance.era));
    }

    private void setRoomEnv(string tag)
    {
        print("Setting view in epoque: " + tag);

        GameObject.FindWithTag(tag).SetActive(true);

        foreach (int era in PlayerEra2.Values)
        {
            string otherTag = PlayerEra2.TagForEra(era);
            if (!otherTag.Equals(tag))
            {
                GameObject.FindWithTag(otherTag).SetActive(false);
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
}
