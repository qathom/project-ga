using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastDoorBehavior : DoorBehavior
{
    [SerializeField]
    private Transform doorTransform;

    public float openDegree;

    public override void OpenDoor()
    {
        Vector3 rotation = doorTransform.localEulerAngles;
        doorTransform.localEulerAngles = new Vector3(rotation.x, openDegree, rotation.z);
    }

    public override void CloseDoor()
    {
        Vector3 rotation = doorTransform.localEulerAngles;
        doorTransform.localEulerAngles = new Vector3(rotation.x, 0f, rotation.z);
    }
}
