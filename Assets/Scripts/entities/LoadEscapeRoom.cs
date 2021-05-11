using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadEscapeRoom : Entity
{
    [SerializeField]
    private int sceneId;
    private bool disabled = false;
    private bool loadingScene = false;

    protected override void Update()
    {
        base.Update();

        disabled = !GameLobbyBrain.Instance.IsReady;
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        Debug.Log("INTERACT");
        Debug.Log(disabled);

        if (!disabled && !loadingScene && PhotonNetwork.LocalPlayer.IsMasterClient)
        {

            Debug.Log("load scene");
            PhotonNetwork.LoadLevel(sceneId);
        }
    }
}
