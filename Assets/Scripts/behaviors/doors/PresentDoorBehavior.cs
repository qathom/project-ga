using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentDoorBehavior : DoorBehavior
{
    [SerializeField]
    private Transform doorTransform;

    public float openDegree = 90;

    public override void OpenDoor()
    {
        Vector3 rotation = doorTransform.localEulerAngles;
        doorTransform.localEulerAngles = new Vector3(rotation.x, rotation.y, openDegree);
    }

    public override void CloseDoor()
    {
        Vector3 rotation = doorTransform.localEulerAngles;
        doorTransform.localEulerAngles = new Vector3(rotation.x, rotation.y, 0f);
    }
}

