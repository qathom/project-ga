using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeEntity : Entity
{
    public PickableEntity entity;

    public override bool CanInteract(PlayerManager playerManager)
    {
        return entity != null && playerManager.CanPickUpEntity;
    }

    public override void Interact(PlayerManager playerManager)
    {
        playerManager.PickUpEntity(entity);
        entity = null;
    }

    public override string GetDescription(PlayerManager playerManager)
    {
        return entity == null ? "Empty Fridge" : "Fridge";
    }
}
