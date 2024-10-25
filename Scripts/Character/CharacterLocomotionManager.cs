using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    public CharacterManager character;

    [Header("GroundCheck & Jumping")]

    [SerializeField] float gravityForce = -5.5f;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity=-20;//force at which character is sticking to the GROUND

    [SerializeField] public bool isRolling=false;
    
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer=0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if(character.isGrounded)
        {
            if (yVelocity.y<0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            if(!character.isJumping && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet= true;
                yVelocity.y = fallStartYVelocity;
            }
            inAirTimer =inAirTimer + Time.deltaTime;
            character.animator.SetFloat("inAirTimer", inAirTimer);

            yVelocity.y += gravityForce * Time.deltaTime;

            
        }
        character.characterController.Move(yVelocity*Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    /*protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }*/

    public void EnableCanRotate()
    {
        character.canRotate = true;
    }
    public void DisbaleCanRotate()
    {
        character.canRotate = false;
    }
}
