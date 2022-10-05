using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PistolGuyMovement : MonoBehaviour
{
    GameObject audio; 


    [SerializeField] private Animator m_anim;
    private Vector3 m_moveVector;
    private CharacterController m_characterController;
   

    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        audio = GameObject.Find("Audio");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveForward();
        }
        else
        {
            Idle();
        }

        var dancing = false; 
        var aiming = false;

        dancing = Input.GetKey(KeyCode.H) ? true : false; 
        aiming = Input.GetKey(KeyCode.G) ? true : false;

         m_anim.SetBool("Dancing", dancing);
         m_anim.SetBool("Aiming", aiming);
    }

    private void MoveForward()
    {
        if (!audio.activeSelf)
            audio.SetActive(true);
        m_anim.SetFloat("Speed", 5);
        m_moveVector = transform.forward;
    }

    private void Idle()
    {
        if (audio.activeSelf)
            audio.SetActive(false);

        m_anim.SetFloat("Speed", 0);
        m_moveVector = Vector3.zero;
    }

    private void FixedUpdate()
    {
        m_characterController.Move(m_moveVector * Time.deltaTime);
    }

}