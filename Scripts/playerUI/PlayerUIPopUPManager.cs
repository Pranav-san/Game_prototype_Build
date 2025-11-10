using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO.Pipes;
using UnityEngine.Rendering;

public class PlayerUIPopUPManager : MonoBehaviour
{
    [Header("Message PopUp")]
    [SerializeField] TextMeshProUGUI popMessageText;
    [SerializeField] public  GameObject interactUIGameObject;
    [SerializeField] public  GameObject popUpMessageGameObject;


    [Header("You Died PopUp")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUPBackGroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUPText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;//Allow us to Set Alpha Fade Over Time

    [Header("Boss Defeated PopUp")]
    [SerializeField] GameObject BossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI BossDefeatedPopUPBackGroundText;
    [SerializeField] TextMeshProUGUI BossDefeatedPopUPText;
    [SerializeField] CanvasGroup BossDefeatedPopUpCanvasGroup;

    [Header("Item PopUp")]
    [SerializeField] GameObject itemPopUpGameObject;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemAmount;
    [SerializeField] CanvasGroup itemPopUpCanvas;
   

    [Header("Site of Grace Restored Pop Up")]
    [SerializeField] GameObject graceRestoredPopUpGameObject;
    [SerializeField] TextMeshProUGUI graceRestoredPopUpBackGroundText;
    [SerializeField] TextMeshProUGUI graceRestoredPopUpText;
    [SerializeField] CanvasGroup graceRestoredPopUpCanvasGroup;

    [Header("Dialogue Pop Up")]
    [SerializeField] GameObject dialoguePopUpGameObject;
    [SerializeField] TextMeshProUGUI dialoguePopUpText;
    [SerializeField] CharacterDialogue currentDialogue;
    [SerializeField] private GameObject continueButton;
    private Coroutine dialogueCoroutine;








    public void SendPlayerMessagePopUp(string messageText)
    {
        PlayerUIManager.instance.popUpWindowIsOpen =true;
        popMessageText.text = messageText;
        
        popUpMessageGameObject.SetActive(true);   

    }

    public void ToggleInteractButton()
    {
        interactUIGameObject.SetActive(true);

    }
    public void SendYouDiedPopUp()
    {


        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUPBackGroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUPBackGroundText, 8, 8.32f));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 4f));
        StartCoroutine(WaitThenFadePopUpOverTime(youDiedPopUpCanvasGroup, 2, 4.5f, youDiedPopUpGameObject));
    }

    public void SendBossDefeatedPopUp(string BossDefeatedMessage)
    {

        BossDefeatedPopUPBackGroundText.text = BossDefeatedMessage;
        BossDefeatedPopUPText.text = BossDefeatedMessage;

        BossDefeatedPopUpGameObject.SetActive(true);
        BossDefeatedPopUPBackGroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(BossDefeatedPopUPBackGroundText, 8, 8.32f));
        StartCoroutine(FadeInPopUpOverTime(BossDefeatedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadePopUpOverTime(BossDefeatedPopUpCanvasGroup, 2, 5, BossDefeatedPopUpGameObject));
    }
    public void SendGraceRestoredPop(string graceRestoredMesage)
    {
        graceRestoredPopUpText.text = graceRestoredMesage;
        graceRestoredPopUpBackGroundText.text = graceRestoredMesage;
        graceRestoredPopUpGameObject.SetActive(true);
        graceRestoredPopUpBackGroundText.characterSpacing= 0;


        StartCoroutine(StretchPopUpTextOverTime(graceRestoredPopUpBackGroundText, 8, 8.32f));
        StartCoroutine(FadeInPopUpOverTime(graceRestoredPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadePopUpOverTime(graceRestoredPopUpCanvasGroup, 2, 5, graceRestoredPopUpGameObject));

        


    }

    public void SendDialoguePopUp(CharacterDialogue dialogue, AICharacterManager aiCharacter)
    {
        PlayerUIManager.instance.playerUIHUDManager.ToggleHUDOnly(false);
        currentDialogue = dialogue;

        if (dialogueCoroutine!=null)
            StopCoroutine(dialogueCoroutine);

       

        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();
        //PlayerUIManager.instance.popUpWindowIsOpen = true;

        dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));

    }

    public void SendNextDialoguePopUpInIndex(CharacterDialogue dialogue, AICharacterManager aiCharacter)
    {
        currentDialogue = dialogue;

        if (dialogueCoroutine !=null)
            StopCoroutine(dialogueCoroutine);

        if(aiCharacter.aiCharacterSoundFXManager.dialogueIsPlaying)
          aiCharacter.aiCharacterSoundFXManager.audioSource.Stop();

        //Close All PopUpWindows
        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();
        //PlayerUIManager.instance.popUpWindowIsOpen = true;

        currentDialogue.dialogueIndex++;
        dialogueCoroutine = StartCoroutine(dialogue.PlayDialogueCoroutine(aiCharacter));

    }

    public void SetDialoguePopUpSubtitles(string dialogueText)
    {
        dialoguePopUpGameObject.SetActive(true);
        dialoguePopUpText.text = dialogueText;

    }

    public void EndDialoguePopUp()
    {
        dialoguePopUpGameObject.SetActive(false);
        PlayerUIManager.instance.playerUIHUDManager.ToggleHUDOnly(true);
    }


    public void OnContinueButtonPressed()
    {

        if (currentDialogue != null)
            currentDialogue.waitingForPlayerInput = false;
    }



    public void SendItemPopUp(Item item, int amount)
    {
        itemAmount.enabled=false;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        if(amount > 1)
        {
            itemAmount.enabled = true;
            itemAmount.text = "x"+amount.ToString();


        }

        itemPopUpGameObject .SetActive(true);
        PlayerUIManager.instance.popUpWindowIsOpen=true;

        StartCoroutine(WaitThenFadePopUpOverTime(itemPopUpCanvas, 0.25f,1.5f, itemPopUpGameObject));

       



    }
    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration<0)
        {
            text.characterSpacing = 0;
            float timer = 0;
            yield return null;

            while (timer<duration)
            {
                timer= timer+ Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration*(Time.deltaTime/20));
                yield return null;
            }
        }

    }
    private  IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)
    {
        if (duration>0)
        {
            canvas.alpha=0;
            float timer = 0;

            yield return null;

            while (timer<duration)
            {
                timer = timer+ Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha,1, duration* Time.deltaTime);
                yield return null;
            }
        }
        canvas.alpha = 1;
        yield return null;
    }

    private IEnumerator WaitThenFadePopUpOverTime(CanvasGroup canvas, float duration, float delay, GameObject objectToDisable = null)
    {

        while (delay>0)
        {
            delay = delay- Time.deltaTime;  
            yield return null;  
        }

        if (duration>0)
        {
            canvas.alpha=1;
            float timer = 0;

            yield return null;

            while (timer<duration)
            {
                timer = timer+ Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration* Time.deltaTime);
                yield return null;
            }
        }
        canvas.alpha = 0;

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

        yield return null;

    }

    public void CloseAllPopUpWindow()
    {
        //Debug.Log("Closing all pop-up windows.");
        popUpMessageGameObject.SetActive(false);
        interactUIGameObject.SetActive(false);
        itemPopUpGameObject.SetActive(false);
        PlayerUIManager.instance.popUpWindowIsOpen = false;

    }

  

  

   


}
