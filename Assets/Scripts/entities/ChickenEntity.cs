using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEntity : Entity
{
    public Entity requiredEntity;
    public Entity resultEntity;
    public RunAway runAway;
    public bool fed = false;

    public override bool CanInteract(PlayerManager playerManager)
    {
        return !fed && playerManager.Entity == requiredEntity;
    }

    public override void Interact(PlayerManager playerManager)
    {
        requiredEntity.AttachTo(transform.parent);
        requiredEntity.gameObject.SetActive(false);
        playerManager.DropEntity();

        resultEntity.gameObject.SetActive(true);
        playerManager.PickUpEntity(resultEntity);
        runAway.IsRunAway = true;
    }

    public override string GetDescription(PlayerManager playerManager)
    {
        if (fed)
        {
            return "Happy Chicken";
        }

        return runAway.IsRunAway ? "Affraid Chicken\nCraves for a carrot!" : "Affraid Chicken\nCraves for a carrot!";
    }
}
