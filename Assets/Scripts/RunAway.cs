using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;

    // TODO
    bool IsRunAway = true;

    Animator anim;
    Rigidbody rb;

    float speed = 15f;
    float minDistance = 3f;
    private float canJump = 0f;
    public float timeBeforeNextJump = 5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsRunAway) {
            EscapePlayer();
            return;
        }

        ChasePlayer();
    }

    private void ChasePlayer()
    {
        var player = PlayerManager.LocalInstance.firstPersonCamera.transform;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > minDistance)
        {
            // Restore collision
            rb.isKinematic = false;

            anim.SetInteger("Walk", 1);
            agent.enabled = true;

            Vector3 pos = Vector3.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);
            rb.MovePosition(pos);
            transform.LookAt(player);

            agent.SetDestination(pos);
        } else {
            anim.SetInteger("Walk", 0);
            agent.enabled = false;
            // Avoid collision
            rb.isKinematic = true;
        }
    }

    private void EscapePlayer() {
        Vector3 playerPosition = PlayerManager.LocalInstance.firstPersonCamera.transform.position;
        float distance = Vector3.Distance(transform.position, playerPosition);

        if (distance > 20) {
            Debug.Log("STOP");

            anim.SetInteger("Walk", 0);
            return;
        }

        if (distance < 5) {
            Debug.Log("JUMP");
            Jump();
            return;
        }

        agent.enabled  = true;
        // Restore collision
        rb.isKinematic = false;

        Debug.Log("RUN");

        anim.SetInteger("Walk", 1);

        Vector3 dirToPlayer = transform.position - playerPosition;
        Vector3 newPos = transform.position + dirToPlayer;

        // rb.MovePosition(newPos);
        agent.SetDestination(newPos);
    }

    private void Jump() {
        /*
        if (Time.time < canJump) {
            Debug.Log("CAN NOT JUMP");
            return;
        }
        */

        canJump = Time.time + timeBeforeNextJump;

        agent.enabled  = false;
        // Ignore collision
        rb.isKinematic = true;

        anim.SetTrigger("jump");

        float jumpSpeed = 5f;
        float height = 0.8f;

        Vector3 pos = transform.position;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * jumpSpeed);
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(pos.x, (newY + 1) * height, pos.z);
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (IsRunAway) {
            Escape();
            return;
        }

        Follow();
    }

    void Escape() {
        Vector3 playerPosition = PlayerManager.LocalInstance.firstPersonCamera.transform.position;

        float distance = Vector3.Distance(transform.position, playerPosition);

        // Debug.log('Distance:' + distance);


        if (distance < distanceRun) {
            Vector3 dirToPlayer = transform.position - playerPosition;
            Vector3 newPos = transform.position + dirToPlayer;

            _agent.SetDestination(newPos);
        }
    }

    void Follow() {
        _agent.SetDestination(PlayerManager.LocalInstance.firstPersonCamera.transform.position);
    }
    */
}
