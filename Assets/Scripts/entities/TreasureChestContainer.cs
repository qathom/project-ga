using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestContainer : Entity
{
    public PickableEntity Entity;
    public GameObject openedChest;
    public GameObject closedChest;
    private bool filled = false;
    public bool Filled
    {
        get { return filled; }
        set
        {
            filled = value;
            if (filled)
            {
                openedChest.SetActive(false);
                closedChest.SetActive(true);
            }
            else
            {
                openedChest.SetActive(true);
                closedChest.SetActive(false);
            }
        }
    }

    protected override void Awake()
    {
        Entity.gameObject.SetActive(false);
        Filled = false;
    }

    public override void Interact(PlayerManager playerManager)
    {
        Entity.gameObject.SetActive(true);
        playerManager.PickUpEntity(Entity);
        Entity = null;
        Filled = false;
    }

    public override bool CanInteract(PlayerManager playerManager)
    {
        return filled && Entity != null && playerManager.CanPickUpEntity;
    }

    public override string GetDescription(PlayerManager playerManager)
    {
        return filled && Entity != null ? "Time Travel Chest" : "Empty Time travel chest";
    }
}
