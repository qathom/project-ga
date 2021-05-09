using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void Start () {
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.E)) {
            print("PICK GRAB");
            grab();
        }
    }

    void OnCollisionEnter (Collision collider) {
        print("PICK1 TRIGGER RANGE OK");

        if (collider.gameObject && collider.gameObject.tag == "Player") {
            print("PICK1 IN RANGE OK");
        }
    }

    void onTriggerEnter(Collider collider) {
        print("PICK TRIGGER RANGE OK");

        if (collider.gameObject && collider.gameObject.tag == "Player") {
            print("PICK IN RANGE OK");
        }
    }

    private void grab()
    {
        Destroy(gameObject);
    }
}
