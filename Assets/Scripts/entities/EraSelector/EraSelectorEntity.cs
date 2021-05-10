using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EraSelectorEntity : Entity
{
    public PlayerEra playerEra = PlayerEra.PAST;
    private bool _used;
    public bool Used
    {
        get { return _used; }
    }

    private PlayerManager localPlayerManager;
    private bool requesting;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        if (!Used && playerManager.era == null)
        {
            Debug.Log("ON");
            playerManager.era = playerEra;
            _used = true;
            photonView.RPC("SetUsed", RpcTarget.Others, true);
        }
        else  if (Used && playerManager.era == playerEra)
        {
            Debug.Log("OFF");
            playerManager.era = null;
            _used = false;
            photonView.RPC("SetUsed", RpcTarget.Others, false);
        }
    }

    [PunRPC]
    void SetUsed(bool used)
    {
        _used = used;
        Debug.LogFormat("set used {0}", used);
    }
}
