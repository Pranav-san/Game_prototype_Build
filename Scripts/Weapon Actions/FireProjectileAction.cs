using UnityEngine;


[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/Fire Projectile")]
public class FireProjectileAction : WeaponItemBasedAction
{

    [SerializeField] ProjectileSlot projectileSlot;
    public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.isGrounded)
            return;

        if(playerPerformingAction.playerStatsManager.currentStamina<=0 )
            return;

        RangedProjectileItem projectileItem = null;

        //Define Which Projectile We are Using
        switch (projectileSlot)
        {
            case ProjectileSlot.Main: projectileItem = playerPerformingAction.playerInventoryManager.mainProjectile;
                break;
            case ProjectileSlot.Secondary: projectileItem = playerPerformingAction.playerInventoryManager.secondaryProjectile;
                break;
            default:
                break;
        }

        if(projectileItem == null )
            return;

        if (!playerPerformingAction.playerEquipmentManager.isTwoHandingWeapon)
        {
           
            PlayerInputManager.Instance.Two_Hand_Input = true;
          

            
        }

        if(!playerPerformingAction.playerInventoryManager.hasArrowNotched)
        {
            playerPerformingAction.playerInventoryManager.hasArrowNotched = true;

            bool canIDrawProjectile = CanIFireThisProjectile(weaponPerformingAction, projectileItem);

            if(projectileItem.currentAmmoAmount <=0)
            {
                //Play Out of Ammo Animation

                return;
            }
            playerPerformingAction.playerCombatManager.currentProjectileBeingUsed = projectileSlot;
            playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Bow_Draw", true);
            playerPerformingAction.playerEquipmentManager.DrawProjectile(projectileItem.itemID);


        }


    }

    private bool CanIFireThisProjectile(WeaponItem weaponPerformingAction, RangedProjectileItem ProjectileItem)
    {
        //Check for weapon and compare it with ammo to give a result 

        return true;

    }

}
