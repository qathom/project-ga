using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent _agent;

    public float distanceRun = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = PlayerManager.LocalInstance.firstPersonCamera.transform.position;

        float distance = Vector3.Distance(transform.position, playerPosition);

        // Debug.log('Distance:' + distance);


        if (distance < distanceRun) {
            Vector3 dirToPlayer = transform.position - playerPosition;
            Vector3 newPos = transform.position + dirToPlayer;

            _agent.SetDestination(newPos);
        }
    }
}
