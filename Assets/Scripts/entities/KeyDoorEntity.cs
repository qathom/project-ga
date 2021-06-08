using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorEntity : Entity, DoorInfo
{
    private bool opened = false;

    [SerializeField]
    private Entity key;

    public override bool CanInteract(PlayerManager playerManager)
    {
        return !opened && playerManager.Entity == key;
    }

    public override void Interact(PlayerManager playerManager)
    {
        playerManager.DropEntity();
        key.AttachTo(transform.parent);
        key.gameObject.SetActive(false);
        opened = true;
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
