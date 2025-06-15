using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItemInteractable : Interactable
{

    public ItemPickUpType pickUpType;

    [Header("Survival Item Type")]
    public SurvivalItemType ItemType;

    [Header("Item")]
    [SerializeField] public Item item;//Pick up Item

    [Header("World Spwan Pickup")]
    [SerializeField] public int itemID;
    [SerializeField] public bool hasBeenLooted;




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
        if (!WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey(itemID))
        {
            WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, false);

        }

        hasBeenLooted = WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted[itemID];

        //If HasBeenLooted, Hide Gameobject
        if(hasBeenLooted)
        {
            gameObject.SetActive(false);

        }
       

    }

    public override void Interact(playerManager player)
    {
        base.Interact(player);

        if (isRayCastInteractable)
        {
            player.characterSoundFxManager.PlaySoundfx(WorldSoundFXManager.instance.pickUpItemSfx);

            //Add item to Inventory
            //player.playerInventoryManager.AddItemToInventory(item);

            //Display a Ui PopUp SHowing item Name & Icon
            PlayerUIManager.instance.playerUIPopUPManager.SendItemPopUp(item, 1);

            //Save Loot Status If its a World Spwan

            if (pickUpType == ItemPickUpType.WorldSpwan)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey((int)itemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(itemID);

                }
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, true);

            }

            //Destroy(gameObject);

        }
        else
        {
            //Play pickUp Sfx
            player.characterSoundFxManager.PlaySoundfx(WorldSoundFXManager.instance.pickUpItemSfx);

            //Add item to Inventory
            player.playerInventoryManager.AddItemToInventory(item);

            //Display a Ui PopUp SHowing item Name & Icon
            PlayerUIManager.instance.playerUIPopUPManager.SendItemPopUp(item, 1);

            //Save Loot Status If its a World Spwan

            if (pickUpType == ItemPickUpType.WorldSpwan)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey((int)itemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(itemID);

                }
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add(itemID, true);

            }

            Destroy(gameObject);
        }

    }

   
    


}
