using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveRandomly : MonoBehaviour
{
    private NavMeshAgent agent; 
    private Animator anim;

    private Vector3 targetDestination;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.stoppingDistance = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            FindNewTargetDestination();
        }

        agent.SetDestination(targetDestination);
        anim.SetFloat("Speed", agent.speed);
    }

    private void FindNewTargetDestination()
    {
        var range = 10f;

        Vector3 randomPoint = transform.position + UnityEngine.Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            targetDestination = hit.position;
        }
        else 
        {
            FindNewTargetDestination();
        }
    }
}
