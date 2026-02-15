using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombatManager : MonoBehaviour
{
    protected CharacterManager character;

    [Header("Last Attack Animation Performed")]
    public string lastAttackAnimationPerformed;

    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("LockOn Transform")]
    public Transform LockOnTransform;  

    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Attack flags")]
    public bool canBlock = true;
    public bool canBeBackStabed = true;
    public bool canBeRiposted = true;

    [Header("Critical Attack")]
    private Transform riposteReceiverTransform;
    private Transform backstabReceiverTransform;
    [SerializeField] float criticalAttackDistance = 0.7f;
    public int pendingCriticalDamage;
    public bool isRipostable = false;

    [Header("Bow")]
    public bool isFiringBow;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();


    }
    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (newTarget != null)
        {
            currentTarget = newTarget;

           

            

        }
        else
        {
            currentTarget = null;
           
        }

    }

    public virtual void AttemptCriticalAttack()
    {

        if(character.isPerformingAction)
            return;
        if (character.characterStatsManager.currentStamina<=0)
            return;

        RaycastHit[] hits = Physics.RaycastAll(character.characterCombatManager.LockOnTransform.position,
         character.transform.TransformDirection(Vector3.forward), criticalAttackDistance, WorldUtilityManager.Instance.GetCharacterLayer());

        //Debug.Log("Attempting Critical Attack");
        for (int i =0; i < hits.Length; i++)
        {
            //Check Each Of The Hits One By One Giving Them Their Own Variable
            RaycastHit hit = hits[i];

            CharacterManager targetCharacter = hit.transform.GetComponent<CharacterManager>();

            if(targetCharacter != null)
            {
                //If the character is the one Attemptimg Critical Strike, Go to the next Hit in The array of Total Hits
                if (targetCharacter==character)
                continue;

                //Debug.Log("Checking " + targetCharacter.name);
                //If We Cannot Damage This Character That Is Targeted Continue To Check The Next Hit In Array
                if (!WorldUtilityManager.Instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                    continue;

                //This Gets Our Position An Angle In Respect To Our Current critical Damage Target 
                Vector3 directionFromCharacterToTarget = character.transform.position - targetCharacter.transform.position;
                float targetViewableAngle =Vector3.SignedAngle(directionFromCharacterToTarget, targetCharacter.transform.forward, Vector3.up);

                Debug.Log($"Target: {targetCharacter.name}, Ripostable: {targetCharacter.characterCombatManager.isRipostable}, Angle: {targetViewableAngle}");


              
                if (targetCharacter.characterCombatManager.isRipostable)
                {
                    if(targetViewableAngle >=-60 &&  targetViewableAngle <= 60)
                    {
                        Debug.Log("Conditions met. Calling AttemptRiposte");
                        AttemptRiposte(hit);
                        character.isPerformingCriticalAttack = true;
                        return;
                    }
                }

                //BackStab Check

                if (targetCharacter.characterCombatManager.canBeBackStabed)
                {
                    if(targetViewableAngle <= 180 && targetViewableAngle >=145)
                    {
                        AttemptBackStab(hit);
                        character.isPerformingCriticalAttack = true;
                        return;
                    }
                    if (targetViewableAngle >= -180 && targetViewableAngle <= -145)
                    {
                        AttemptBackStab(hit);
                        character.isPerformingCriticalAttack = true;
                        return;
                    }
                }


            }
        }
    }

    public virtual void AttemptRiposte(RaycastHit hit)
    {
        Debug.Log("RIPOSTING TARGET WOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO!");

       






    }


    public virtual void AttemptBackStab(RaycastHit hit)
    {
        Debug.Log("Back Stabing TARGET!");








    }

    public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemyCharacter, Vector3 localOffset)
    {
        float timer = 0;

        // Create a local transform relative to the character (this)
        if (riposteReceiverTransform == null)
        {
            GameObject riposteTransformObject = new GameObject("Riposte Transform");
            riposteTransformObject.transform.SetParent(transform); // parent to character
            riposteTransformObject.transform.localPosition = localOffset; // e.g., (0, 0, 0.5)
            riposteTransformObject.transform.localRotation = Quaternion.identity;
            riposteReceiverTransform = riposteTransformObject.transform;
        }
        else
        {
            riposteReceiverTransform.localPosition = localOffset;
        }

        // Move ENEMY to riposte position (calculated in world space)
        Vector3 targetPosition = riposteReceiverTransform.position;
        targetPosition.y = enemyCharacter.transform.position.y; // lock to ground
        enemyCharacter.transform.position = targetPosition;

        // Rotate PLAYER to face enemy
        Vector3 direction = enemyCharacter.transform.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        // Wait briefly before animation
        while (timer < 0.2f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }


    public IEnumerator ForceMoveEnemyCharacterToBackStabPosition(CharacterManager enemyCharacter, Vector3 localOffset)
    {
        float timer = 0;

        // Create a local transform relative to the character (this)
        if (backstabReceiverTransform == null)
        {
            GameObject backstabTransformObject = new GameObject("BackStab Transform");
            backstabTransformObject.transform.SetParent(transform); // Parent to character
            backstabTransformObject.transform.localPosition = localOffset;
            backstabTransformObject.transform.localRotation = Quaternion.identity;
            backstabReceiverTransform = backstabTransformObject.transform;
        }
        else
        {
            backstabReceiverTransform.localPosition = localOffset;
        }

        // Move enemy to the proper backstab position (relative to character)
        Vector3 targetPosition = backstabReceiverTransform.position;
        targetPosition.y = enemyCharacter.transform.position.y; // Keep grounded
        enemyCharacter.transform.position = targetPosition;

        // Rotate character to face enemy's back
        Vector3 dir = enemyCharacter.transform.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        // Wait briefly before animation
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }


    public void EnableRipostable()
    {
        isRipostable=true;
        

    }

    public void OnIsLockedOnChanged(bool old, bool isLoackeOn)
    {
        currentTarget = null;

    }

    public void EnableIsInvulnerable()
    {
        character.isInvulnerable = true;

    }

    public void DisableIsInvulnerable()
    {
        character.isInvulnerable = false;

    }

    public virtual void ApplyCriticalDamage()
    {
        character.characterEffectsManager.PlayCriticalBloodSplatterVFX(character.characterCombatManager.LockOnTransform.position);
        character.characterSoundFxManager.PlayCriticalStrikeSfx();

        character.characterStatsManager.currentHealth = 0;
    }


    public virtual void CancelCurrentAttack()
    {
        DisableAllDamageColliders();
    }

    protected virtual void DisableAllDamageColliders() 
    { 

    }


    public virtual void OnWeaponRecoil()
    {
       
    }
}
