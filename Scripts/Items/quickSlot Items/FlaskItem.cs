using UnityEngine;


[CreateAssetMenu(menuName = "Items/Consumeables/Life Flask")]
public class FlaskItem : QuickSlotItem
{

    [Header("Flask Type")]
    public bool isHealingFlask = true;

    [Header("Flask Restoration value")]
    [SerializeField] int flaskRestoration = 25;




    [Header("Empty item")]
    public GameObject emptyFlaskGameObjct;
    public string emptyFlaskAnimation;


    public override bool CanIuseThisItem(playerManager player)
    {
        if(!player.playerCombatManager.isUsingItem&& player.isPerformingAction)
            return false;
        if(player.isAttacking)
            return false;

        

        return true;
       
    }


    public override void AttemptToUseItem(playerManager player)
    {
        if(!CanIuseThisItem(player)) 
            return;

        //Health Flask check
        if (isHealingFlask && player.playerInventoryManager.remainingHealthFlasks<=0)
        {
            if(player.playerCombatManager.isUsingItem)
                return;
            player.playerCombatManager.isUsingItem = true;

            player.playerAnimatorManager.PlayTargetActionAnimation(emptyFlaskAnimation, false, false, true, true, false);
            Destroy(player.playerEffectsManager.activeQuickSlotItemFx);
            GameObject emptyFlask = Instantiate(emptyFlaskGameObjct, player.playerEquipmentManager.rightWeaponHandSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFx = emptyFlask;

            return;

        }

        if (player.playerCombatManager.isUsingItem)
        {
            player.animator.SetBool("isChuggingFlask", true);
            return;
        }
        player.playerCombatManager.isUsingItem = true;
           
        player.playerEffectsManager.activeQuickSlotItemFx = Instantiate(itemModel, player.playerEquipmentManager.rightWeaponHandSlot.transform);

        player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true,true,false);

        player.HideWeaponmodel();


       
    }

    public override void SuccessfullyUsedItem(playerManager player)
    {
        base.SuccessfullyUsedItem(player);

        if (isHealingFlask)
        {
            player.playerStatsManager.currentHealth += flaskRestoration;
            player.playerInventoryManager.remainingHealthFlasks -= 1;
            PlayerUIManager.instance.UpdateHealthBar(Mathf.RoundToInt(player.playerStatsManager.currentHealth));
            PlayerUIManager.instance.RefreshAllUIAfterItemUse();

        }

        if(isHealingFlask&&player.playerInventoryManager.remainingHealthFlasks <= 0)
        {
            Destroy(player.playerEffectsManager.activeQuickSlotItemFx);
            GameObject emptyFlask = Instantiate(emptyFlaskGameObjct,player.playerEquipmentManager.rightWeaponHandSlot.transform);
            player.playerEffectsManager.activeQuickSlotItemFx = emptyFlask;

        }

        playHealingFx();
           
    }

    public override int GetCurrentAmount(playerManager player)
    {
        int currentAmount = 0;

        if (isHealingFlask)
        {
           currentAmount = player.playerInventoryManager.remainingHealthFlasks;

           

        }
        return currentAmount;


    }

    private void playHealingFx()
    {

    }

}
