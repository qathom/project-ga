using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EraSelectorEntity : Entity
{
    public int era = PlayerEra2.PAST;

    public bool Used
    {
        get { return GameLobbyBrain.Instance.PlayerForEra(era) >= 0; }
    }

    private bool Mine
    {
        get { return GameLobbyBrain.Instance.PlayerForEra(era) == PhotonNetwork.LocalPlayer.ActorNumber; }
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

    public override bool CanInteract()
    {
        return !Used || Mine;
    }

    public override string GetDescription()
    {
        if (!Used)
        {
            return PlayerEra2.Name(era) + " Era Selector";
        }
        else if (Mine)
        {
            return PlayerEra2.Name(era) + " Era affected to you";
        }
        else
        {
            return PlayerEra2.Name(era) + " Era allready affected";
        }
    }
}
