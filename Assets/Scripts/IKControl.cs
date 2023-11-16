using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))] 

public class IKControl : MonoBehaviour {
    
    protected Animator m_animator;
    
    [SerializeField]
    private Transform RightHandTransform;
    [SerializeField]
    private Transform LeftHandTransform;

    public bool ikActive = false;
    public Transform RightHandObj = null;
    public Transform LeftHandObj = null;
    public Transform LookObj = null;

    void Start () 
    {
        m_animator = GetComponent<Animator>();
    }
    
    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if(m_animator) 
        {
            //if the IK is active, set the position and rotation directly to the goal. 
            if(ikActive) 
            {
                // Set the look target position, if one has been assigned
                if(LookObj != null) 
                {
                    m_animator.SetLookAtWeight(1);
                    m_animator.SetLookAtPosition(LookObj.position);
                }    

                // Set the right hand target position and rotation, if one has been assigned
                if(RightHandObj != null) 
                {
                    m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
                    m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
                    m_animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandObj.position);
                    m_animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(RightHandObj.right));
                }        

                if(LeftHandObj != null) 
                {
                    m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,1);
                    m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,1);  
                    m_animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandObj.position);
                    m_animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(LeftHandObj.right));
                    m_animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(-LeftHandObj.right));
                }        
            }
            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else 
            {          
                m_animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                m_animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0);
                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0); 
                m_animator.SetLookAtWeight(0);
            }
        }
    }    
}
