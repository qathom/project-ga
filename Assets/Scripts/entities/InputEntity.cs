using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEntity : Entity
{
    public override void Interact(PlayerManager playerManager)
    {
        playerManager.OpenInputOverlay();
    }

    public virtual bool SendInput(PlayerManager playermanager, string input)
    {
        return false;
    }
}
