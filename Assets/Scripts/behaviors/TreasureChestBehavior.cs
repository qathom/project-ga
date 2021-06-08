using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TreasureChestBehavior : MonoBehaviourPun
{
    public TreasureChestEntity sender;
    public TreasureChestContainer receiver;

    public int Era { get { return PlayerManager.LocalInstance.era; } }
    private bool sent = false;

    public void Update()
    {
        if (!sent && Era == PlayerEra2.PRESENT && sender.entity != null)
        {
            sent = true;
            photonView.RPC("Send", RpcTarget.OthersBuffered);
        }
    }

    #region Pun RPCs

    [PunRPC]
    public void Send()
    {
        if (Era == PlayerEra2.PAST)
        {
            receiver.Filled = true;
        }
    }

    #endregion
}
