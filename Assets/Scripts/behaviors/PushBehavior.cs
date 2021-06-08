using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBehavior : MonoBehaviour
{
    public float Force = 1f;

    public void Push(Vector3 direction, Rigidbody rigidbody)
    {
        if (rigidbody == null) return;

        rigidbody.AddForce(direction * Force);
    }
}
