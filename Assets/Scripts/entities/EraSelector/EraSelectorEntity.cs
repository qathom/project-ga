using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EraSelectorEntity : Entity
{
    public int era = PlayerEra2.PAST;

    private bool _used;
    public bool Used
    {
        get { return _used; }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        _used = GameLobbyBrain.Instance.PlayerForEra(era) >= 0;
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        int currentPlayer = GameLobbyBrain.Instance.PlayerForEra(era);

        int player = PhotonNetwork.LocalPlayer.ActorNumber;
        int playerEra = GameLobbyBrain.Instance.EraForPlayer(player);

        if (currentPlayer < 0 && playerEra < 0)
        {
                GameLobbyBrain.Instance.photonView.RPC(
                    "PlayerJoinOrLeaveEra", RpcTarget.MasterClient,
                    era, player, true);
        }
        else if (currentPlayer == player)
        {
                GameLobbyBrain.Instance.photonView.RPC(
                    "PlayerJoinOrLeaveEra", RpcTarget.MasterClient,
                    era, player, false);
        }
    }
}
