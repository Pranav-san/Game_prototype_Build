using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    public CharacterManager character;


    [Header("GroundCheck & Jumping")]

    public float gravityForce = -5.5f;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] public Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity=-20;//force at which character is sticking to the GROUND

    [SerializeField] public bool isRolling=false;
    
    [SerializeField] protected float fallStartYVelocity = -5;
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer=0;

    [Header("Ladder")]
    public bool isClimbingLadder = false;
    public LadderInteractable currentLadderInteractable;

    [Header("Slope Sliding")]
    [SerializeField] float slopeSlideStartPositionYOffeset = 1;
    [SerializeField] float slopeSlideSphereCastMaxdistance = 2;
    private Vector3 slopeSlideVelocity;
    [SerializeField] float slopeSlideSpeedMultiplier = 3;
    [SerializeField] float slipperySurfaceMaxAngle = 15;
    private bool isSliding =false;
    private bool isSlidingOffCharacter = false;
    private Coroutine slideOffCharacterCoroutine;
    private bool slideUntilGrounded = false;
    [SerializeField] float characterCollisionCheckSphereMultiplier = 1.5f;
    [SerializeField] float characterSlideOffCollisionMaxDistance = 5f;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

    }

    protected virtual void Update()
    {

        
        HandleGroundCheck();
        SetGroundedVelocity();
        HandleSlopeSlideCheck();

        if (isClimbingLadder)
        {
            // skip gravity and normal movement while climbing
            yVelocity = Vector3.zero;
            return;
        }
        if (character.isGrounded)
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
        if (character.isGrounded)
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

            if (!character.isGrounded)
                OnIsNotGrounded();
        }
        else
        {
            // DEPENDING ON YOUR CHARACTER SETUP, SOMETIMES MAKING THE GROUND CHECK SPHERE RADIUS DIFFERENT WHILST NOT GROUNDED HAS BENEFITS
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer, QueryTriggerInteraction.Ignore);

            if(yVelocity.y > 0 )
            {
                character.isGrounded = false;
                return;

            }
            if(character.isGrounded)
                OnIsGrounded();

        }
       
    }


    //SLOPE SLIDING
    private void HandleSlopeSlideCheck()
    {
        if(slopeSlideVelocity == Vector3.zero)
            isSliding = false;

        if(!character.isGrounded&& slideUntilGrounded)
        {
            SetSlopeSlideVelocity(WorldUtilityManager.Instance.GetEnviroLayer());
            return;
        }

        if(!character.isGrounded)
            return;

        SetSlopeSlideVelocity(WorldUtilityManager.Instance.GetSlipperyEnviroLayer());



    }


    //This Function Determines What Our SlopeSlide velocity Will Be When Not Grounded
    private void SetSlopeSlideVelocity(LayerMask layers)
    {
        Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + slopeSlideStartPositionYOffeset, transform.position.z);

        //Use a Sphere Cast To Get Whats Below Us/Character, And If The Angle Is Too Great, We Apply Slope Slide Velocity
        if(Physics.SphereCast
            (startPosition,groundCheckSphereRadius, Vector3.down, out RaycastHit hitinfo, slopeSlideSphereCastMaxdistance, layers))
        {
            float angle = Vector3.Angle(hitinfo.normal,Vector3.up);
            slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0,slopeSlideSpeedMultiplier,0),hitinfo.normal);

            if(angle >= slipperySurfaceMaxAngle)
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, slopeSlideSpeedMultiplier, 0), hitinfo.normal);
                return;

            }

        }
        //Otherwise Set Slope Slide Velocity to Zero
        else
        {
            slopeSlideVelocity = Vector3.zero;
        }

        if(isSliding)
        {
            slopeSlideVelocity -= slopeSlideVelocity*Time.deltaTime * slopeSlideSpeedMultiplier;

            if(slopeSlideVelocity.magnitude > 1)
                return;
        }

        slopeSlideVelocity = Vector3.zero;


    }

    private void SetGroundedVelocity()
    {
        //If Ignoring Gravity Do Not Process This Function

        if (slopeSlideVelocity != Vector3.zero)
        {
            //If You Are In The Process Of Jump, And Your Jump Is Still Gaining Height, Do Not Slide Of Surfaces
            if (character.isJumping && yVelocity.y > 0)
            {
                isSliding = false;
            }
            else
            {
                isSliding = false;
            }

            if (isSliding)
            {
                yVelocity.y = WorldUtilityManager.Instance.slopeSlideForce*Time.deltaTime;
                Vector3 slideVelocity = slopeSlideVelocity;

                if (character.characterController.enabled)
                    character.characterController.Move(slideVelocity*Time.deltaTime);

            }
            if (character.isGrounded)
            {
                if (yVelocity.y <=0 && !isSliding)
                    yVelocity.y = groundedYVelocity;

            }
            else if (!character.isGrounded&& !isSlidingOffCharacter)
            {
                {
                    //Handle Sliding Of Character
                    Collider[] characterColliders = Physics.OverlapSphere(transform.position, groundCheckSphereRadius*
                        characterCollisionCheckSphereMultiplier, WorldUtilityManager.Instance.GetCharacterLayer());

                    for (int i = 0; i < characterColliders.Length; i++)
                    {
                        //If The Character Is Us Ignore And Continue
                        if (characterColliders[i].gameObject.transform.root == character.gameObject.transform.root)
                            continue;
                        CharacterController controller = characterColliders[i].GetComponent<CharacterController>();

                        if(controller == null)
                            continue;

                        if((controller.collisionFlags & CollisionFlags.CollidedBelow) != 0)
                        {
                            isSlidingOffCharacter = true;

                           


                        }

                    }


                }

                if (!character.characterController.enabled)
                    return;

            }
        }
    }


    protected virtual void OnIsGrounded()
    {
        // FALL DAMAGE
        // YOU COULD DETERMINE HOW HIGH YOU FELL BY SAVING A POSITION WHEN YOU LEAVE THE GROUND, AND SAVING ONE WHEN YOU LAND
        // COMPARE THE Y LEVEL OF THESE POSITIONS AND IF YOU WERE IGNORING GRAVITY OR NOT
        // IF THE Y LEVEL IS TOO GREAT, APPLY DAMAGE ACCORDINGLY
        // PLAYING AN IMPACT/LANDING ANIMATION

        slideUntilGrounded = false;

    }

    protected virtual void OnIsNotGrounded()
    {

    }

    protected void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!character.isGrounded)
            slideUntilGrounded= true;
    }


    //CHARACTER SLIDING
    protected virtual void SlideOffCharacter()
    {
        if(slideOffCharacterCoroutine !=null)
            StopCoroutine(slideOffCharacterCoroutine);

        slideOffCharacterCoroutine = StartCoroutine(SlideOffCoroutine());


    }

    protected virtual IEnumerator SlideOffCoroutine()
    {

        while (!character.isGrounded)
        {
            if (Physics.SphereCast(character.transform.position, groundCheckSphereRadius, Vector3.down, out RaycastHit hitInfo, 
                characterSlideOffCollisionMaxDistance, WorldUtilityManager.Instance.GetCharacterLayer()))
            {
                Vector3 characterSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, yVelocity.y, 0), hitInfo.normal);
                yVelocity.y += WorldUtilityManager.Instance.slopeSlideForce* Time.deltaTime;
                Vector3 slideVelocity = characterSlideVelocity;
                if (character.characterController.enabled)
                    character.characterController.Move(slideVelocity * Time.deltaTime);
                yield return null;

            }
            yield return null;

        }
        isSlidingOffCharacter = false;

        yield return null;

    }


    //protected void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    //}



    public void EnableCanRotate()
    {
        
        character.canRotate = true;
    }
    public void DisbaleCanRotate()
    {
        
        character.canRotate = false;
    }
}
