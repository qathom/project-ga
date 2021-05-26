using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerEntity : Entity
{
    [HideInInspector]
    public Entity entity = null;
    public bool HasEntity { get { return entity != null; } }

    [SerializeField]
    private Transform container;

    public override void Interact(PlayerManager playerManager)
    {
        entity = playerManager.Entity;
        playerManager.DropEntity();
        entity.AttachTo(null);
        updateEntityPosition();
    }

    public override bool CanInteract(PlayerManager playerManager)
    {
        return CanContainPlayerEntity(playerManager);
    }

    public virtual bool CanContainPlayerEntity(PlayerManager playerManager)
    {
        return playerManager.Entity != null && !HasEntity;
    }

    public override string GetDescription(PlayerManager playerManager)
    {
        return HasEntity ? "Container" : "EmptyContainer";
    }

    private void updateEntityPosition()
    {
        entity.transform.position = container.transform.position;
    }
}
