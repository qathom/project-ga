using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    [SerializeField]
    private Object doorInfoRef;
    private DoorInfo doorInfo;

    private bool openState;

    private void Awake()
    {
        if (doorInfoRef is DoorInfo)
        {
            doorInfo = (DoorInfo) doorInfoRef;
        }
        else
        {
            Debug.LogError("Door Behavior: wrong door info ref type");
        }
    }

    private void Start()
    {
        openState = doorInfo.isOpen();
    }

    // Update is called once per frame
    void Update()
    {
        bool isOpen = doorInfo.isOpen();

        if (openState != isOpen)
        {
            openState = doorInfo.isOpen();

            if (isOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }
    }

    public virtual void OpenDoor()
    {
        print("open");
    }

    public virtual void CloseDoor()
    {
        print("close");
    }
}
