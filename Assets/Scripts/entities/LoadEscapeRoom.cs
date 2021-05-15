using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadEscapeRoom : Entity
{
    [SerializeField]
    private int sceneId;
    private bool loadingScene = false;

    public override void Interact(PlayerManager playerManager)
    {
        loadingScene = true;
        PhotonNetwork.LoadLevel(sceneId);
    }

    public override bool CanInteract()
    {
        return !loadingScene && PhotonNetwork.LocalPlayer.IsMasterClient && GameLobbyBrain.Instance.IsReady;
    }

    public override string GetDescription()
    {
        return "Start Game\nInteract when every era has been affected.";
    }
}
