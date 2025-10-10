using UnityEngine;

public class AICharacterSoundFXManager : CharacterSoundFxManager
{

    AICharacterManager aiCharacter;

    [Header("Dialogues")]
    public characterDialogueID characterDialogueID;
    public GameObject interactableDialogueGameObject;
    public CharacterDialogue currentDialogue;
    public GameObject dialogueInteractableGameObject;
    public bool dialogueIsPlaying = false;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
    }

    protected override void Start()
    {
        base.Start();

        currentDialogue = WorldSaveGameManager.instance.GetCharacterDialogueByEnum(characterDialogueID);
    }


    //Dialogue
    public void PlayCurrentDialogueEvent()
    {
        if(currentDialogue == null)
            return;

        if(!dialogueIsPlaying)
        {
            currentDialogue.PlayDialogueEvent(aiCharacter);
        }
        else
        {
            PlayerUIManager.instance.playerUIPopUPManager.SendNextDialoguePopUpInIndex(currentDialogue, aiCharacter);
        }



    }

    //Generic Farewell Dialogues That Can Be Changed With Different Sets
   

    //Cancels Current Dialogue Event (Used When Exiting Trigger Area)
    public void CancelCurrentDialogueEvent()
    {

    }

    //Used For Specific Calls When The Dialogue is Over(Shop Opens, Npc Dies, Etc)
    public void OnCurrentDialogueEnded()
    {
        currentDialogue = WorldSaveGameManager.instance.GetCharacterDialogueByEnum(characterDialogueID);

    }
    
}
