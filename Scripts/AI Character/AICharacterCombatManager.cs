using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{

    protected AICharacterManager aiCharacter;

    [Header("Damage")]
     protected int baseDamage = 25;
     protected int basePoiseDamage = 25;

    

    

    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetDirection;

    [Header("Stance Setings")]
    public float maxStance = 150f;
    public float currentStance;
    public float stanceRegeneratedPerSecond = 15;
    [SerializeField] bool ignoreStanceBreak = false;

    [Header("Stance timer")]
    public float stanceRegenerationTimer = 0;
    [SerializeField] private float  stancetickTimer = 0;
    [SerializeField] float deafaultTimeUntilStanceRegenerationBegins = 15;

    [Header("Grapple Hold Position")]
    public Transform grappleTransfom;









    [Header("Detection")]
    [SerializeField] float detectionRadius = 15f;
    public float minimumDetectionFOV = -35f;
    public  float maximumDetectionFOV = 35f;

    [Header("PivotRotation Type")]
    [SerializeField] bool eightRotations = false;
    [SerializeField] bool fourRotations = true;

    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed = 25;

    [Header("AI Equipments")]
    public ProjectileSlot currentProjectileBeingUsed;

    [Header("Debug")]
    [SerializeField] bool investigateSound = false;
    [SerializeField] Vector3 positionofSound;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();

        currentStance = maxStance;

        
    }

    private void Update()
    {
        HandleStanceBreak();

       
    }

    private void HandleStanceBreak()
    {
        if (aiCharacter.characterStatsManager.isDead)
            return;

        if(stanceRegenerationTimer > 0)
        {
            stanceRegenerationTimer -= Time.deltaTime;
        }
        else
        {
            stanceRegenerationTimer = 0;

            if(currentStance < maxStance)

            {
                //Begin Adding Stance Each Tick
                stancetickTimer +=Time.deltaTime;

                if(stancetickTimer >=1)
                {
                    stancetickTimer =0f;
                    currentStance += stanceRegeneratedPerSecond;
                }

            }

            else
            {
                currentStance = maxStance;
            }
        }

        if(currentStance <=0)
        {
            currentStance = maxStance;

            if(ignoreStanceBreak)
                return;

            aiCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Stance Break_01", true);
        }
        

    }

    public void DamageStance(int stanceDamage)
    {
        //When Stance Is Damaged, The Timer Is Reset, Constant Attacks Gives No Chnace At Recovering Stance That Is Lost
        stanceRegenerationTimer =deafaultTimeUntilStanceRegenerationBegins;

        currentStance -= stanceDamage;


    }

    public void AwardRunesOnDeath(playerManager player)
    {
        if (player.characterGroup == CharacterGroup.Team02)
            return;

        player.playerStatsManager.AddRunes(aiCharacter.characterStatsManager.runesDroppedOnDeath);
        Debug.Log("Awarding runes: " + aiCharacter.characterStatsManager.runesDroppedOnDeath);

    }

    public void FindTargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if(currentTarget!=null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayer());

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter == null)
                continue;
            if(targetCharacter == aiCharacter) 
                continue;

            if(targetCharacter.characterStatsManager.isDead) 
                continue;

            if(WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                //IF a potential Target is found it has to be infront of us
                Vector3 targetdirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetdirection, aiCharacter.transform.forward);

                if(angleOfPotentialTarget > minimumDetectionFOV && angleOfPotentialTarget < maximumDetectionFOV)
                {
                    //Check for Enviro Blocks
                    if(Physics.Linecast(aiCharacter.characterCombatManager.LockOnTransform.position, 
                        targetCharacter.characterCombatManager.LockOnTransform.position, 
                        WorldUtilityManager.Instance.GetEnviroLayer()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.LockOnTransform.position, targetCharacter.characterCombatManager.LockOnTransform.position, Color.red, 2f );
                        //Debug.Log("Blocked");
                    }
                    else
                    {
                        targetDirection=targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);

                        if (eightRotations)
                        {
                            PivotTowardsTarget(aiCharacter);
                        }
                        
                        else if (fourRotations)
                        {
                            PivotTowardsTargetFourRotations(aiCharacter);
                        }
                            

                    }
                }
            }
            

        }


    }

    //Rotatate and Face Target Using Animations
    public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;

        // Right turns
        if (viewableAngle >= 20 && viewableAngle <= 60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 45", true);
            Debug.Log("45 degree Right");
        }
        else if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 90", true);
            Debug.Log("90 degree Right");
        }
        else if (viewableAngle >= 111 && viewableAngle <= 160)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 135", true);
            Debug.Log("135 degree Right");
        }
        else if (viewableAngle >= 161 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 180", true);
            Debug.Log("180 degree Right");
        }

        // Left turns
        else if (viewableAngle <= -20 && viewableAngle >= -60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 45", true);
            Debug.Log("45 degree Left");
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 90", true);
            Debug.Log("90 degree Left");
        }
        else if (viewableAngle <= -111 && viewableAngle >= -160)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 135", true);
            Debug.Log("135 degree Left");
        }
        else if (viewableAngle <= -161 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 180", true);
            Debug.Log("180 degree Left");
        }
    }

    public virtual void PivotTowardsTargetFourRotations(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;

        // Right turns
       
        if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 90", true);
            Debug.Log("90 degree Right");
        }
        else if (viewableAngle >= 161 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 180", true);
            Debug.Log("180 degree Right");
        }

        // Left turns
        
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 90", true);
            Debug.Log("90 degree Left");
        }
        
        else if (viewableAngle <= -161 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 180", true);
            Debug.Log("180 degree Left");
        }
    }

    public virtual void PivotTowardsPosition(AICharacterManager aiCharacter, Vector3 position)
    {
        if (aiCharacter.isPerformingAction)
            return;

        Vector3 targetDirection = position - aiCharacter.transform.position;
        float viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(aiCharacter.transform, targetDirection);

        // Right turns
        if (viewableAngle >= 20 && viewableAngle <= 60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 45", true);
            Debug.Log("45 degree Right");
        }
        else if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 90", true);
            Debug.Log("90 degree Right");
        }
        else if (viewableAngle >= 111 && viewableAngle <= 160)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 135", true);
            Debug.Log("135 degree Right");
        }
        else if (viewableAngle >= 161 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 180", true);
            Debug.Log("180 degree Right");
        }

        // Left turns
        else if (viewableAngle <= -20 && viewableAngle >= -60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 45", true);
            Debug.Log("45 degree Left");
        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 90", true);
            Debug.Log("90 degree Left");
        }
        else if (viewableAngle <= -111 && viewableAngle >= -160)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 135", true);
            Debug.Log("135 degree Left");
        }
        else if (viewableAngle <= -161 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 180", true);
            Debug.Log("180 degree Left");
        }
    }


    public virtual void AlertCharacterToSound(Vector3 positionOfSound)
    {
        if(aiCharacter.characterStatsManager.isDead)
            return;

        if(aiCharacter.idle == null)
            return;

        if(aiCharacter.investigateSound == null)
            return ;

        if(!aiCharacter.idle.willInvestigateSound)
            return ;

        //If they are Sleeping, Here is Where You Wake Them Up
       




        aiCharacter.investigateSound.positionOfSound = positionOfSound;
        aiCharacter.currentState = aiCharacter.currentState.SwitchState(aiCharacter, aiCharacter.investigateSound);

    }


    public override void CancelCurrentAttack()
    {
        base.CancelCurrentAttack();
    }



    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if (actionRecoveryTimer>0)
        {
            if(!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;  
            }
            else if(aiCharacter.isHoldingArrow)
            {
                actionRecoveryTimer -= Time.deltaTime;



            }

        }
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        //Sebbs Logic
        if(aiCharacter.isMoving)
        {
        aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }



       
    }

    public void RotateTowardsTargetWhilestAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget==null)
            return;

        if(!aiCharacter.canRotate)
            return;

        

        

        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection==Vector3.zero)
        {
            targetDirection = aiCharacter.transform.forward;  
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);

        //Debug.Log("Rotate While Attacking");


    }

    public void ReleaseArrow()
    {

        aiCharacter.isHoldingArrow = false;









        //Animate The Bow
        //Play Fire Animation
        //bowAnimator.SetBool("isDrawn", false);
        //bowAnimator.Play("Bow_Fire");
        aiCharacter.canMove = false;

        //Projectile we are Firing
        RangedProjectileItem projectileItem = null;

        switch (currentProjectileBeingUsed)
        {
            case ProjectileSlot.Main:
                projectileItem = aiCharacter.aiCharacterInventoryManager.mainProjectile;
                break;
            case ProjectileSlot.Secondary:
                projectileItem = aiCharacter.aiCharacterInventoryManager.secondaryProjectile;
                break;
            default:
                break;
        }

        if (projectileItem == null)
            return;

        if (projectileItem.currentAmmoAmount <=0)
            return;

        Transform projectileInstantiationLocation;
        GameObject projectileGameObject;
        Rigidbody projectileRigidbody;
        RangedProjectileDamageCollider projectileDamageCollider;

        //Subtract Ammo
        projectileItem.currentAmmoAmount -= 1;

        projectileInstantiationLocation = aiCharacter.aiCharacterCombatManager.LockOnTransform;
        projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
        projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
        projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

        //Formula To Set Damage
        projectileDamageCollider.physicalDamage = 20;
        projectileDamageCollider.characterShootingProjectile = aiCharacter;




        //Locked And Not Aiming
        if (aiCharacter.aiCharacterCombatManager.currentTarget != null)
        {
            Quaternion arrowRotation = Quaternion.LookRotation(aiCharacter.aiCharacterCombatManager.currentTarget.characterCombatManager.LockOnTransform.position
                - projectileGameObject.transform.position);

            projectileGameObject.transform.rotation = arrowRotation;


        }
    





        //Get All Character Damage Collider and Ignore Self

        Collider[] characterColliders = aiCharacter.GetComponentsInChildren<Collider>();
        List<Collider> collidersArrowWillIgnore = new List<Collider>();

        foreach (var item in characterColliders)
            collidersArrowWillIgnore.Add(item);

        foreach (Collider hitBox in collidersArrowWillIgnore)
            Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);

        projectileRigidbody.AddForce(projectileGameObject.transform.forward* projectileItem.forwardVelocity);
        projectileGameObject.transform.parent = null;











    }

    public void DrawProjectile()
    {
        aiCharacter.isHoldingArrow = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

    }

    public virtual void PerformEvasion()
    {
        if(currentTarget==null)
            return;
        if(distanceFromTarget>5)
            return;

        Debug.Log("AI Evaded Attack");

        //Method #1 A.I Simply play Dodge Animation
        //aiCharacter.isInvulnerable = true;
        //aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Roll Backward", true);

        //Method #2 A.I Rolls Away From Target

        //aiCharacter.isInvulnerable = true;
        //aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Roll Backward", true);
        //Vector3 directionToDodge = -aiCharacter.transform.forward;
        //directionToDodge.y = 0;
        //directionToDodge.Normalize();

        //Optionally use Couroutine To Apply this Rotation 

        //aiCharacter.transform.rotation = Quaternion.LookRotation(directionToDodge);

        // //Method #3 Choose a Random Direction And Roll Toward It 
        aiCharacter.isInvulnerable = true;
        aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Roll", true);
        Vector3 directionToDodge = Random.insideUnitSphere.normalized;
        directionToDodge.y = 0;
        aiCharacter.transform.rotation = Quaternion.LookRotation(directionToDodge);


    }
}
