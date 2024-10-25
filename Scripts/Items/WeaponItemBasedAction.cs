using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="CharacterActions/WeaponActions/Test Actions")]
public class WeaponItemBasedAction : ScriptableObject
{

    public int actionId;


    

    public virtual void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        playerPerformingAction.isUsingRightHand = true;

        playerPerformingAction.playerCombatManager.currentWeaponBeingUsed = weaponPerformingAction;


        
        Debug.Log("The Action Has Fired");



    }

    
}
