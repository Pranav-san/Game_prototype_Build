using UnityEngine;


[CreateAssetMenu(menuName = "Items/Consumeables/ Bomb Item")]
public class BombItem : QuickSlotItem
{
    [Header("Velocity")]
    public int upwardVelocity = 50;
    public int forwardVelocity = 50;
    public int itemWeight;

    [Header("Damage")]
    public int baseDamage = 200;

    [Header("Bomb Item")]
    public GameObject liveBombItem;
    public string emptyAnimation;


    public override void AttemptToUseItem(playerManager player)
    {
        if (player.playerInventoryManager.remainingBombs>0)
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, false, false, true, true, false);
            player.playerEffectsManager.activeQuickSlotItemFx = Instantiate(itemModel, player.playerEquipmentManager.rightWeaponHandSlot.transform);

        }
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(emptyAnimation, false, false, true, true, false);

        }
    }


    public override void SuccessfullyUsedItem(playerManager player)
    {
        Destroy(player.playerEffectsManager.activeQuickSlotItemFx);


    }




}
