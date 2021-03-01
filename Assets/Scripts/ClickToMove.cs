using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent agent; 
    private Animator anim;

    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 5.5f;



    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
             RaycastHit hit;
            
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                agent.destination = hit.point;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            agent.speed = runSpeed;
        } 
        else
        {
            agent.speed = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
        }

        SetAnimationParameters();
    }

    private void SetAnimationParameters()
    {
        anim.SetFloat("Speed", agent.desiredVelocity.magnitude);
    }
}
