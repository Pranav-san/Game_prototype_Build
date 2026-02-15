using UnityEngine;

[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/Fire Gun (Projectile)")]
public class FireGunProjectileAction : WeaponItemBasedAction
{
    [SerializeField] ProjectileSlot projectileSlot;
    public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.isGrounded)
            return;

        if(!playerPerformingAction.isAiming)
            return;


        RangedProjectileItem projectileItem = null;

        //Define Which Projectile We are Using
        switch (projectileSlot)
        {
            case ProjectileSlot.Main:
                projectileItem = playerPerformingAction.playerInventoryManager.mainProjectile;
                break;
           
            default:
                break;
        }

        if (projectileItem == null)
            return;

        

        if (!playerPerformingAction.playerEquipmentManager.isTwoHandingWeapon)
        {

            PlayerInputManager.Instance.Two_Hand_Input = true;



        }
        bool canIDrawProjectile = CanIFireThisProjectile(weaponPerformingAction, projectileItem);
        
        if (projectileItem.currentMagazineAmmo <=0)
        {
            //Play Out of Ammo Animation


            if (projectileItem.currentAmmoAmount>0)
            {
                playerPerformingAction.playerAnimatorManager.EnableDisableIK(0, 0);
                playerPerformingAction.playerInventoryManager.FireBullet = false;
                playerPerformingAction.animator.SetBool("FireBullet", playerPerformingAction.playerInventoryManager.FireBullet);
                playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimationInstantly("ReloadWeapon", true);
               
                

            }
            else
            {
                //Play Out of Ammo Animation

            }
            return;
        }

        playerPerformingAction.playerCombatManager.currentProjectileBeingUsed = projectileSlot;
        playerPerformingAction.playerInventoryManager.FireBullet = true;
        playerPerformingAction.animator.SetBool("FireBullet", playerPerformingAction.playerInventoryManager.FireBullet);







    }

    private bool CanIFireThisProjectile(WeaponItem weaponPerformingAction, RangedProjectileItem ProjectileItem)
    {
        //Check for weapon and compare it with ammo to give a result 

        return true;

    }
}
