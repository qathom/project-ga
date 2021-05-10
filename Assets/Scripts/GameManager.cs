using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public static class RoomEnv
{
    public const string Medieval = "Medieval";
    public const string Present = "Present";
    public const string Future = "Future";
}

public class GameManager : MonoBehaviourPunCallbacks
{

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    private void Start()
    {

        if (PlayerManager.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);

            // Display the right "epoque" or room environment
            //setRoomEnv();
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    /*
     * Sets the room environment depending on the "epoque"
     */
    private void setRoomEnv()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            enableFor(RoomEnv.Medieval);
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            enableFor(RoomEnv.Present);
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            enableFor(RoomEnv.Future);
            return;
        }
    }

    private void enableFor(string tagName)
    {
        print("Setting view in epoque: " + tagName);

        string[] arrTags = { RoomEnv.Medieval, RoomEnv.Present, RoomEnv.Future };

        GameObject.FindWithTag(tagName).SetActive(true);

        for (int i = 0; i < arrTags.Length; i++)
        {
            string groupTagName = arrTags[i];

            if (groupTagName != tagName)
            {
                GameObject.FindWithTag(groupTagName).SetActive(false);
            }
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {

    }

    public override void OnPlayerLeftRoom(Player other)
    {

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
