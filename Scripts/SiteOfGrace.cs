using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteOfGrace : Interactable
{

    [Header("Site of grace")]
    public int siteOfGraceID;

    [Header("VFC")]
    [SerializeField] GameObject activatedParticles;


    public bool isActivated = false;


    [Header("Interaction text")]
    [SerializeField] string unctivatedInteractionText = "Restore Site of Grace";
    [SerializeField] string activatedInteractionText = "Rest";

    [Header("Teleport Transform")]
    [SerializeField] Transform teleportTransform;

    protected override void Start()
    {
        base.Start();

        if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
        {
            isActivated = WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID];
        }
        else
        {
            isActivated = false;
        }

        if(isActivated)
        {
            //Play Fx, Enable Light... Etc
            activatedParticles.SetActive(true);
        }
        if (isActivated)
        {
            interactableText = "REST";

        }
        else
        {
            interactableText = "LIGHT CAMPFIRE";
        }


    }



    private void RestoreSiteOfGrace(playerManager player)
    {

        isActivated = true;

        //If Our Save File Contains Info About this site of Grace, Remove it
        if (WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.ContainsKey(siteOfGraceID))
        {
            WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Remove(siteOfGraceID);
        }
        //Then Re-Add it with the vale of TRUE (isActivated)
        WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace.Add(siteOfGraceID, true);

        player.playerAnimatorManager.PlayTargetActionAnimation("Activate_Site_Of_Grace_01", true);

        PlayerUIManager.instance.playerUIPopUPManager.SendGraceRestoredPop("CAMPFIRE LIT");

        activatedParticles.SetActive(true);
        
        if (isActivated)
        {
            interactableText = "REST";
        }
        else
        {
            interactableText = "LIT CAMPFIRE";
        }

        StartCoroutine(WaitForAnimationAndPopUpAndRestoreInteractableCollider());

        

        


    }
    private void RestAtSiteOfGrace(playerManager player)
    {
        Debug.Log("Resting");

        PlayerUIManager.instance.playerUISiteOfGraceManager.OpenSiteOfGraceManagerMenu();

        interactableCollider.enabled  = true; //Temporarily Enable for testing

        WorldAIManager.instance.ResetAllCharacters();

        player.playerStatsManager.currentHealth = player.playerStatsManager.maxHealth;
        player.playerStatsManager.currentStamina = player.playerStatsManager.maxStamina;
        PlayerUIManager.instance.UpdateHealthBar(player.playerStatsManager.currentHealth);

        player.isMoving = true;


    }

    

    private IEnumerator WaitForAnimationAndPopUpAndRestoreInteractableCollider()
    {
        yield return new WaitForSeconds(2);//Should give Enough Time For Animation to Play

        interactableCollider.enabled = true;
    }

    

    public override void Interact(playerManager player)
    {
        base.Interact(player);

        if (!isActivated)
        {
            RestoreSiteOfGrace(player);
        }
        else
        {
            RestAtSiteOfGrace(player);
        }

        
    }

}
