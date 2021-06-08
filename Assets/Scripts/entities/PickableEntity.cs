using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableEntity : Entity
{
    private bool pickedUp = false;
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    public Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);

    public override void Interact(PlayerManager playerManager)
    {
        pickedUp = true;
        playerManager.PickUpEntity(this);
    }

    public override void AttachTo(Transform parent)
    {
        base.AttachTo(parent);
        transform.localPosition = offset;
        transform.localScale = scale;
    }

    public override bool CanInteract(PlayerManager playerManager)
    {
        return !pickedUp && playerManager.CanPickUpEntity;
    }
}
