using UnityEngine;



public class PickUpRunesInteractable : Interactable
{

    public int runes = 0;

    public override void Interact(playerManager player)
    {
        WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = false;
        player.playerStatsManager.AddRunes(runes);
        Destroy(gameObject);
    }



}
