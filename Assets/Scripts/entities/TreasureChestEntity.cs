using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestEntity : ContainerEntity
{
    public Entity ExpectedEntity;
    public GameObject openedChest;
    public GameObject closedChest;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        openedChest.SetActive(false);
        closedChest.SetActive(true);
    }

    public override bool CanContainPlayerEntity(PlayerManager playerManager)
    {
        return base.CanContainPlayerEntity(playerManager)
            && playerManager.Entity == ExpectedEntity;
    }

    public override string GetDescription(PlayerManager playerManager)
    {
        return HasEntity ? "Time Travel Chest" : "Empty Time travel chest";
    }
}
