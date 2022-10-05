using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementTS : MonoBehaviour
{
    [SerializeField] private float m_walkSpeed;
    [SerializeField] private float m_runSpeed;
    [SerializeField] private float m_turnSpeed;

    [SerializeField] private Transform m_modelTransform;
    [SerializeField] private Plane m_plane;

    [SerializeField] private Animator m_anim;

    private CharacterController m_characterController;
    private float m_currentMoveSpeed;
    private Vector2 m_moveVector;
    private float m_gunHeight = 1.65f; // distance from ground to weapon


    private void Awake()
    {
        m_plane = new Plane(Vector3.up, new Vector3(transform.position.x, transform.position.y + m_gunHeight, transform.position.z));
        m_characterController = GetComponent<CharacterController>();

        m_currentMoveSpeed = m_walkSpeed;
    }

    private void Update()
    {
        // var h = InputManager.Instance.GetHorizontal();
        // var v = InputManager.Instance.GetVertical();
        // m_currentMoveSpeed = InputManager.Instance.isSprinting() ? m_runSpeed : m_walkSpeed; 

        var h = Input.GetAxis("Horizontal"); 
        var v = Input.GetAxis("Vertical"); 
        m_currentMoveSpeed = Input.GetKeyDown(KeyCode.LeftShift) ? m_runSpeed : m_walkSpeed; 

   
        m_moveVector = new Vector2(h, v).normalized;

        // Vector3 localMovement = transform.InverseTransformDirection(m_moveVector);
        // Vector3 desiredForward = Vector3.RotateTowards(transform.forward, localMovement, m_turnSpeed * Time.deltaTime, 0f);

        // m_anim.SetFloat("Horizontal", localMovement.x);
        // m_anim.SetFloat("Vertical", localMovement.y);

        m_anim.SetFloat("Horizontal", h);
        m_anim.SetFloat("Vertical", v);


        if (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0)
            m_anim.SetBool("Moving", true);
        else
            m_anim.SetBool("Moving", false);
        // var mousePos = Mouse.current.position.ReadValue();
        var mousePos = Input.mousePosition;

        RotateTowardMouse(mousePos);
    }

    private void RotateTowardMouse(Vector2 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        float hitDist = 0f;

        if (m_plane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            m_modelTransform.rotation = Quaternion.Slerp(m_modelTransform.rotation, targetRotation, 7f * Time.deltaTime);

        }
    }

    private void FixedUpdate()
    {
        Move(m_moveVector);
    }

    private void Move(float h, float v) => Move(new Vector2(h, v));

    private void Move(Vector2 move) => m_characterController.Move(new Vector3(move.x, 0.0f, move.y) * m_currentMoveSpeed * Time.deltaTime);

}