using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    // This Script Process Damage, Block Damage, Poision Damage Healing...And
    // Anything That Affects character is processed Here


    public CharacterManager character;

    [Header("Current Active Fx")]
    public GameObject activeQuickSlotItemFx;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterVFX;
    [SerializeField] GameObject criticalBloodloodSplatterVFX;
    [SerializeField] GameObject blockedVFX;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }


    //Can be used to destroy effects models (Drinking Estus, Having Arrow Drawn Etc
    public virtual void InterruptEffect()
    {
        
        //Destroy The Current activeQuickSlotItem Being Used 
        if (activeQuickSlotItemFx != null)
        {
            Destroy(activeQuickSlotItemFx);
        }

        //
        if (character.isHoldingArrow)
        {
            character.isHoldingArrow = false;
            character.animator.SetBool("isHoldingArrow", false);
            Animator rangedWeaponAnimator = character.GetComponentInChildren<Animator>();

            //Fire or Remove Arrom if Bow and play Ranged Weapon Fire Animation If weapon Animator Is present
            if(rangedWeaponAnimator != null)
            {
                //Animate The Bow
                //Play Fire Animation
                //rangedWeaponAnimator.SetBool("isDrawn", false);
                //rangedWeaponAnimator.Play("Bow_Fire");
               
            }
            else
            {
                Debug.Log("Unable To Fetch Weapon Animator");
            }

        }
        //Remove Player From Aiming State If They Are Currently Aiming
        if (character.isAiming)
        {
            character.isAiming = false;
            PlayerCamera.instance.OnIsAimingChanged(false);
        }

    }
    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {

        //character.TriggerHitPause(0.05f);
        effect.ProcessEffect(character);



    }

    public void PlayBloodSplatterVFX(Vector3 contactPoint)
    {
        if(bloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterVFX,contactPoint,Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }

    }

   

    public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)
    {
        if (criticalBloodloodSplatterVFX != null)
        {
            GameObject bloodSplatter = Instantiate(criticalBloodloodSplatterVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.criticalBloodloodSplatterVFX, contactPoint, Quaternion.identity);
        }

    }

    public void PlayBlockedVFX(Vector3 contactPoint)
    {
        if (blockedVFX != null)
        {
            GameObject bloodSplatter = Instantiate(blockedVFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
        }

    }

    


}
