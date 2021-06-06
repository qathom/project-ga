using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameEntity : Entity
{
    private bool triggered = false;

    public override void Interact(PlayerManager playerManager)
    {
        triggered = true;
        GameManager.Instance.sendPlayerEscapedMessage();
    }

    public override bool CanInteract(PlayerManager playerManager)
    {
        return !triggered;
    }

    public override string GetDescription(PlayerManager playerManager)
    {
        return "Congratulation!\nYou resolved the room.\nInteract to show others that you are done!";
    }
}
