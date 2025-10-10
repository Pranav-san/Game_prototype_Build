using UnityEngine;

public class DialogueInteractable : Interactable
{
    AICharacterManager aiCharacter;
    public override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponentInParent<AICharacterManager>();
        

    }

    public override void Interact(playerManager player)
    {

        if (PlayerUIManager.instance.menuWindowOpen)
            return;

        if(aiCharacter.characterStatsManager.isDead)
        {
            interactableCollider.enabled = false;
            return;
        }

        //Play Current Dialogue
        MobileControls.instance.DisableMobileControls();
        
        aiCharacter.aiCharacterSoundFXManager.PlayCurrentDialogueEvent();


    }

    public override void OnTriggerEnter(Collider other)
    {
        if (aiCharacter.characterStatsManager.isDead)
        {
            interactableCollider.enabled = false;


            playerManager player = other.GetComponent<playerManager>();
            if (player!=null)
                aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();
            

        }
        base.OnTriggerEnter(other);


    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);


        playerManager player = other.GetComponent<playerManager>();

        if(player==null)
            return;


        aiCharacter.aiCharacterSoundFXManager.CancelCurrentDialogueEvent();


    }

}
    

