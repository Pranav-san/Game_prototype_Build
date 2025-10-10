using UnityEngine;

[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/Aim Action")]
public class AimAction : WeaponItemBasedAction
{
   
    public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);


        if (!playerPerformingAction.isGrounded)
            return;
        
           

        if(playerPerformingAction.playerCombatManager.isLockedOn)
            return;




        

        playerPerformingAction.isAiming = true;
        playerPerformingAction.animator.SetBool("isAiming", playerPerformingAction.isAiming);

        if(weaponPerformingAction.weaponClass == WeapomClass.Gun)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Gun_Aim", true);
           
        }
        
        PlayerCamera.instance.OnIsAimingChanged(true);





    }
}
