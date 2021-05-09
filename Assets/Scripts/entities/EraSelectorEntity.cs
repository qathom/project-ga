using System;
using UnityEngine;

public class EraSelectorEntity : Entity
{
    public PlayerEra playerEra = PlayerEra.PAST;
    private PlayerManager playerManager;

    public bool Used
    {
        get { return playerManager != null; }
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        Debug.LogFormat("INTERACT {0}, {1}", playerManager, this);

        if (this.playerManager == null)
        {
            playerManager.era = playerEra;
            this.playerManager = playerManager;
        }
        else if (this.playerManager == playerManager)
        {
            playerManager.era = null;
            this.playerManager = null;        
        }

        Debug.Log(playerManager.era);
    }
}
