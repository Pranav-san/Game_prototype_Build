using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_YellowBar : MonoBehaviour
{

    public Slider slider;
    UI_Character_HP_Bar parentHPBar;

    public float timer;


    private void Awake()
    {
        slider = GetComponent<Slider>();
        parentHPBar = GetComponentInParent<UI_Character_HP_Bar>();
    }

    private void OnEnable()
    {
        if (timer<=0)
        {
            timer =1f;
        }
    }

    private void Update()
    {
        if(timer<=0)
        {
            if(slider.value > parentHPBar.slider.value)
            {
                slider.value=  slider.value-0.5f;
            }
            else if(slider.value <= parentHPBar.slider.value)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            timer = timer-Time.deltaTime;
        }
    }



}
