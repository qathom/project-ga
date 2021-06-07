using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureBehavior : MonoBehaviour
{
    public RunAway runAway;
    public Entity selfEntity;

    // Update is called once per frame
    void Update()
    {
        runAway.IsRunAway = PlayerManager.LocalInstance.Entity != selfEntity;
    }
}
