using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatBar : MonoBehaviour
{
    
    public Slider slider;

    


    //[Header("Bar Options")]
    //[SerializeField]protected bool scaleBarLengthWithStats = true;
    //[SerializeField] protected float widthScaleMultiplier = 1;


    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
       


    }

   

    public virtual void Start()
    {

    }

    public virtual void SetStat(int newValue)
    {
        slider.value = newValue;

    }
    public virtual void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;

    }


}
