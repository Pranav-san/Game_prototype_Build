using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    protected Collider damageCollider;


    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float firelDamage = 0;
    public float lightininglDamage = 0;

    [Header("Poise Damage")]
    public float poiseDamage = 0;

    [Header("Contact Point")]
    public Vector3 contactPoint;

    [Header("Block")]
    protected Vector3 directionFromAttackToDamageTarget;
    protected float dotvalueFromAttackToDamageTarget;


   [Header("Characters Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    protected virtual void Awake()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other. GetComponentInParent<CharacterManager>();

        
        // IF YOU WANT TO SEARCH ON BOTH THE DAMAGEABLE CHARACTER COLLIDERS & THE CHARACTER CONTROLLER COLLIDER JUST CHECK FOR NULL HERE AND DO THE FOLLOWING
        /*if (damageTarget == null)
        {
            damageTarget other.GetComponent<CharacterManager>();
        }*/



        if (damageTarget != null)
        {
            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
           

        }

        CheckForBlock(damageTarget);
        DamageTarget(damageTarget);



    }

    protected virtual void CheckForBlock(CharacterManager damageTarget)
    {
        //If character Has already been damaged, do not proceed 
        if(charactersDamaged.Contains(damageTarget))
            return;

       
        GetBlockingDotValues(damageTarget);

        //Check if the character being damaged isBlocking
        if(damageTarget.isBlocking && dotvalueFromAttackToDamageTarget >0.3f)
        {
            Debug.Log("Check if the character being damaged isBlocking");
            //if the Character is blocking Check if they are facing the correct direction to block sucessfully
            charactersDamaged.Add(damageTarget);
            
            TakeBlockedDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeBlockedDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.firelDamage = firelDamage;
            damageEffect.lightininglDamage = lightininglDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.poiseDamage = poiseDamage;

            //Aplly Blocked Damage To The Target
            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

        }

    }

    protected virtual void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget = transform.position - damageTarget.transform.position;
        dotvalueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);

    }

    protected virtual void DamageTarget(CharacterManager damageTarget)
    {

        if (charactersDamaged.Contains(damageTarget)){
            return;
        }
        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.firelDamage = firelDamage;
        damageEffect.lightininglDamage = lightininglDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.poiseDamage = poiseDamage;

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);



    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;

    }
    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear();

    }



}
