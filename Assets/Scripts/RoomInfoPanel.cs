using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomInfoPanel : MonoBehaviour
{
    public Text roomNameText;
    public Text playerCountText;
    public Button joinButton;

    private RoomInfo room;
    private Launcher launcher;

    public void UpdateRoomInfo(RoomInfo roomInfo, Launcher launcher)
    {
        this.launcher = launcher;
        this.room = roomInfo;

        roomNameText.text = roomInfo.Name;
        playerCountText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        joinButton.enabled = roomInfo.IsOpen;
    }

    public void JoinRoom()
    {
        launcher.JoinRoom(room);
    }
}
