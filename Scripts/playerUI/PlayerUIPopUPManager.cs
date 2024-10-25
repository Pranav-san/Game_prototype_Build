using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIPopUPManager : MonoBehaviour
{
    [Header("Message PopUp")]
    [SerializeField] TextMeshProUGUI popMessageText;
    [SerializeField] public  GameObject popUpMessageGameObject;


    [Header("You Died PopUp")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUPBackGroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUPText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;//Allow us to Set Alpha Fade Over Time

    [Header("Item PopUp")]
    [SerializeField] GameObject itemPopUpGameObject;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemAmount;






    public void SendPlayerMessagePopUp(string messageText)
    {
        PlayerUIManager.instance.popUpWindowIsOpen =true;
        popMessageText.text = messageText;
        popUpMessageGameObject.SetActive(true);   

    }
    public void SendYouDiedPopUp()
    {


        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUPBackGroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUPBackGroundText, 8, 8.32f));
        StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadePopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
    }

    public void SendItemPopUp(Item item, int amount)
    {
        itemAmount.enabled=false;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        if(amount > 1)
        {
            itemAmount.enabled=true;
            itemAmount.text = "x"+amount.ToString();


        }

        itemPopUpGameObject .SetActive(true);
        PlayerUIManager.instance.popUpWindowIsOpen=true;



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

    private IEnumerator WaitThenFadePopUpOverTime(CanvasGroup canvas, float duration, float delay)
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
        yield return null;

    }

    public void CloseAllPopUpWindow()
    {
        //Debug.Log("Closing all pop-up windows.");
        popUpMessageGameObject.SetActive(false);
        itemPopUpGameObject.SetActive(false);
        PlayerUIManager.instance.popUpWindowIsOpen = false;

    }


}
