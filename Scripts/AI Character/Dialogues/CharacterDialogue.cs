using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu (menuName ="A.I/Dialogue")]
public class CharacterDialogue : ScriptableObject
{
    [Header("Dialogue Requirements")]
    public int requiredStageID = 0;

    [Header("Greeting Dialogue")]
    [TextArea] public List<string> greetingDialogueString = new List<string>();
    public List<AudioClip> greetingDialogueAudio = new List<AudioClip>();
    private bool greetingHasPlayed = false;

    [Header("Core Dialogue")]
    [TextArea] public List<string> dialogueString = new List<string>();
    public List<AudioClip> DialogueAudio = new List<AudioClip>();
    public int dialogueIndex = 0;

    [Header("Greeting Dialogue")]
    [TextArea] public List<string> farewellDialogueString = new List<string>();
    public List<AudioClip> farewellDialogueAudio = new List<AudioClip>();
    private bool farewellHasPlayed = false;


    [Header("End Triggers")]
    [SerializeField] bool setStageIndex = false; //This Will Be Used To Set The Stage ID, After Setting an ID, New Dialogue Will Be Selected Depending On ID
    [SerializeField] int stageID = 0;


    public void PlayDialogueEvent(AICharacterManager aiCharacterManager)
    {
        if(dialogueString.Count != DialogueAudio.Count)
        {
            Debug.Log("Audioclip Count Does Not Match Subtitle Count");

        }
        aiCharacterManager.aiCharacterSoundFXManager.dialogueIsPlaying = true;
        PlayerUIManager.instance.playerUIPopUPManager.SendDialoguePopUp(this, aiCharacterManager);

    }

    public IEnumerator PlayDialogueCoroutine(AICharacterManager aiCharacter)
    {
        if (greetingDialogueAudio.Count != 0  && !greetingHasPlayed)
        {
            greetingHasPlayed = true;
            int randomGreetingDialogueIndex = Random.Range(0, greetingDialogueAudio.Count);
            PlayerUIManager.instance.playerUIPopUPManager.SetDialoguePopUpSubtitles(greetingDialogueString[randomGreetingDialogueIndex]);
            aiCharacter.aiCharacterSoundFXManager.PlaySoundfx(greetingDialogueAudio[randomGreetingDialogueIndex]);

            yield return new WaitForSeconds(greetingDialogueAudio[randomGreetingDialogueIndex].length+1);
        }

        dialogueIndex = 0;
        while (dialogueIndex < dialogueString.Count)
        {
            PlayerUIManager.instance.playerUIPopUPManager.SetDialoguePopUpSubtitles(dialogueString[dialogueIndex]);
          
            yield return new WaitForSeconds(3f);

            dialogueIndex++;
        }

        if (!farewellHasPlayed)
        {
            farewellHasPlayed = true;
            int randomGreetingDialogueIndex = Random.Range(0, farewellDialogueString.Count);
            PlayerUIManager.instance.playerUIPopUPManager.SetDialoguePopUpSubtitles(farewellDialogueString[randomGreetingDialogueIndex]);
            //aiCharacter.aiCharacterSoundFXManager.PlaySoundfx(greetingDialogueAudio[randomGreetingDialogueIndex]);

            yield return new WaitForSeconds(3f);
        }



        OnDialogueEnded(aiCharacter);
        PlayerUIManager.instance.playerUIPopUPManager.EndDialoguePopUp();
       
        yield return null;
    }

    public void OnDialogueEnded(AICharacterManager aiCharacter)
    {
        //Do Stuff With Character Dialogue Scriptable
        greetingHasPlayed = false;
        dialogueIndex = 0;
        aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying = false;
        MobileControls.instance.EnableMobileControls();
        

        if (setStageIndex)
            WorldSaveGameManager.instance.SetStageOfDialogue(aiCharacter.aiCharacterSoundFXManager.characterDialogueID, stageID);


        //Do Stuff with AI Character
        aiCharacter.aiCharacterSoundFXManager.OnCurrentDialogueEnded();

    }

    public void DialogueCancelled(AICharacterManager aiCharacter)
    {
        
    }



}
