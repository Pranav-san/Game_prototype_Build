using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInteractable : Interactable
{

    public ItemPickUpType pickUpType;

   

    [Header("Item")]
    [SerializeField] public Item item;//Pick up Item
   

    [Header("World Spwan Pickup")]
    [SerializeField] public int worldSpwanInteractableItemID;// This Is An Unique ID Given To Each World Spwan Item
    [SerializeField] public bool hasBeenLooted;

    [Header("Creature Loot Pick Up")]
    public int itemID;
    public Vector3 itemPosition;





    protected override void Start()
    {
        base.Start();

        if(pickUpType == ItemPickUpType.WorldSpwan)
        {
            CheckIfWorldItemWasAlreadyLooted();

        }

        
    }

    private void CheckIfWorldItemWasAlreadyLooted()
    {
        if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(worldSpwanInteractableItemID))
        {
            WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(worldSpwanInteractableItemID, false);

        }

        hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[worldSpwanInteractableItemID];

        //If HasBeenLooted, Hide Gameobject
        if(hasBeenLooted)
        {
            gameObject.SetActive(false);

        }
       

    }

    public override void Interact(playerManager player)
    {
        base.Interact(player);
        
        //Play pickUp Sfx
        player.characterSoundFxManager.PlaySoundfx(WorldSoundFXManager.instance.pickUpItemSfx);
        
        //Add item to Inventory
        player.playerInventoryManager.AddItemToInventory(item);
        
        //Display a Ui PopUp SHowing item Name & Icon
        if(item is RangedProjectileItem)
        {
            RangedProjectileItem Projectileitem = item as RangedProjectileItem;
            PlayerUIManager.instance.playerUIPopUPManager.SendItemPopUp(Projectileitem, Projectileitem.pickUpAmount);

        }
        else
        {
            PlayerUIManager.instance.playerUIPopUPManager.SendItemPopUp(item, 1);

        }
       
        
        //Save Loot Status If its a World Spwan
        if (pickUpType == ItemPickUpType.WorldSpwan)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey((int)worldSpwanInteractableItemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(worldSpwanInteractableItemID);

                }
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(worldSpwanInteractableItemID, true);

            }

        Destroy(gameObject);
        

    }

    public void SetItemPosition(Vector3 position)
    {
        if (pickUpType != ItemPickUpType.CharacterDrop)
            return;

        WorldItemDatabase.instance.GetItemByID(itemID);

        transform.position = position;
    }

   
    


}
