using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(menuName = "CharacterActions/WeaponActions/Heavy Attack Action ")]
    public class HeavyAttackWeaponItemAction : WeaponItemBasedAction
    {
        [SerializeField] string heavy_Attack_01 = "Heavy_Attack_01";
        [SerializeField] string heavy_Attack_02 = "Heavy_Attack_02";
        public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            //Check for Stops
            if (!playerPerformingAction.isGrounded)
                return;

            playerPerformingAction.isAttacking = true;


            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(playerManager playerPerformingAction, WeaponItem WeaponPerformingAction)
        {
            //If we are Attacking Currently, And We Can Combo,Perform The Combo Attack
            
            //Otherwise if we are not attacking, Just Perform Regular Attack
             if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(WeaponPerformingAction, AttackType.HeavyAttack01, heavy_Attack_01, true);

            }

        }


    }


