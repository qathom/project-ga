using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomInfoPanel : MonoBehaviour
{
    public Text roomNameText;
    public Text playerCountText;
    public Button joinButton;

    private RoomInfo room;

    public void UpdateRoomInfo(RoomInfo roomInfo)
    {
        this.room = roomInfo;

        roomNameText.text = roomInfo.Name;
        playerCountText.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        joinButton.enabled = roomInfo.IsOpen;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(room.Name);
    }
}
