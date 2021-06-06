using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeDoorEntity : InputEntity, DoorInfo
{
    private bool opened = false;
    public bool Opened { get { return opened; } }

    [SerializeField]
    private string input = "";

    public override bool CanInteract(PlayerManager playerManager)
    {
        return !opened;
    }

    public override bool SendInput(PlayerManager playermanager, string input)
    {
        opened = input != null && input.Equals(this.input);
        return opened;
    }

    public override string GetDescription(PlayerManager playerManager)
    {
        return opened ? "Unlocked Door" : "Locked Door";
    }

    public override string GetInteractionHint(PlayerManager playerManager)
    {
        return "Press 'e' to unlock!";
    }

    public bool isOpen()
    {
        return opened;
    }
}
