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
        setRoomEnv(PlayerManager.LocalInstance.era);
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
}
