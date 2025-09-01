using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGrappleCollider : DamageCollider
{
    AICharacterManager characterCausingDamage;
    


    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        characterCausingDamage = GetComponentInParent<AICharacterManager>();
        damageCollider.enabled = false;


    }

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        {
            if(!damageTarget.isPerformingAction)
            {
                
                
                
                //Playe Enemy Grapple Animation 
                characterCausingDamage.characterAnimatorManager.PlayTargetActionAnimationInstantly("Grapple_Hold", true,true);

                //Play Player Grapple Reaction Animation 
                damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("zombie_grapple_victim_01", true,true);

               


                Quaternion targetZombieRotation = Quaternion.LookRotation(characterCausingDamage.transform.position - damageTarget.transform.position);
                Quaternion targetPlayerRotation = Quaternion.LookRotation(damageTarget.transform.position-characterCausingDamage.transform.position);

                characterCausingDamage.transform.rotation = targetPlayerRotation;

              
                damageTarget.transform.rotation = targetZombieRotation;

                //damageTarget.isGrappled = true;

               


            }

        }
    }


}
