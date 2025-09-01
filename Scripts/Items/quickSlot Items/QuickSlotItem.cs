using System.Diagnostics.Contracts;
using UnityEngine;

public class QuickSlotItem : Item
{

    [Header("item model")]
    [SerializeField] protected GameObject itemModel;

    [Header("Consumable")]
    public bool isConsumable = true;
   

    [Header("Animations")]
    [SerializeField] protected string useItemAnimation;

    public virtual void AttemptToUseItem(playerManager player)
    {
        if(!CanIuseThisItem(player))
             return;

        player.playerAnimatorManager.PlayTargetActionAnimation(useItemAnimation, true);

    }

    public virtual void SuccessfullyUsedItem(playerManager player)
    {


    }

    public virtual bool CanIuseThisItem(playerManager player)
    {
        return true;
    }


    public virtual int GetCurrentAmount(playerManager player)
    {
        return 0;


    }
   
}
