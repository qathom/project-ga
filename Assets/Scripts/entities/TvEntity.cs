using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvEntity : Entity
{
    public GameObject hint;
    public bool on = false;

    protected override void Start()
    {
        base.Start();

        updateHintVisibility();
    }

    public override void Interact(PlayerManager playerManager)
    {
        on = !on;
        updateHintVisibility();
    }

    private void updateHintVisibility()
    {
        hint.SetActive(on);
    }
}
