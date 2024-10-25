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

    [Header("Contact Point")]
    public Vector3 contactPoint;

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
        DamageTarget(damageTarget);



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
