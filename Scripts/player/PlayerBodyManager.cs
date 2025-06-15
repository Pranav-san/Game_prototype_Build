using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyManager : MonoBehaviour
{
    //Enable Body Parts

    [Header("Hair game object")]
    [SerializeField] public GameObject hair;
    [SerializeField] public GameObject facialHair;

    [Header("Male")]
    [SerializeField] public GameObject maleHead;  //Default Head Model when Unequipping Armor or Outfit
    [SerializeField] public GameObject[] maleBody;//Default UpperBodyModel When Unequipping Armor or Outfit
    [SerializeField] public GameObject[] maleArms;//Default UpperBodyModel When Unequipping Armor or Outfit(Lower left arm, Left Hand, Lower Right Arm, Right Hand)
    [SerializeField] public GameObject[] maleLegs;//Default Hips, Left and Right Leg When Unequipping Armor or Outfit
    [SerializeField] public GameObject[] DefaultUpperBodyCloth; //Vest...Etc
    [SerializeField] public GameObject[] DefaultLowerBodyCloth;//LoinCloth, Briefs..Etc

    [SerializeField] public GameObject maleEyes;
    [SerializeField] public GameObject[] maleFacialHair;

    

    [Header("Female")]
    [SerializeField] public GameObject femaleHead;
    [SerializeField] public GameObject[] femaleBody;
    [SerializeField] public GameObject[] femaleArms;
    [SerializeField] public GameObject[] femaleLegs;
    [SerializeField] public GameObject femaleEyes;
   

    public void EnableHead()
    {
        //Enable Head Object
        maleHead.SetActive(true);
        //femaleHead.SetActive(true);

        //Enable Facial Features
        maleEyes.SetActive(true);
        //femaleEyes.SetActive(true);


    }
    public void DisableHead()
    {
        //Enable Head Object
        maleHead.SetActive(false);
        //femaleHead.SetActive(false);

        //Enable Facial Features
        maleEyes.SetActive(false);
        //femaleEyes.SetActive(false);


    }
    public void EnableHair()
    {
        hair.SetActive(true);
    }
    public void DisableHair()
    {
        hair.SetActive(false);
    }
    public void DisableFacialHair()
    {
        facialHair.SetActive(false);
    }
    public void EnableFacialHair()
    {
        facialHair.SetActive(false);
    }

    public void EnableBody()
    {
        foreach(var model in maleBody)
        {
            model.SetActive(true);
        }
    }

    public void DisableBody()
    {
        foreach (var model in maleBody)
        {
            model.SetActive(false);
        }
    }

    public void EnableLowerBody()
    {
        foreach (var model in maleLegs)
        {
            model.SetActive(true);
        }

    }

    public void DisableLowerBody()
    {
        foreach (var model in maleLegs)
        {
            model.SetActive(false);
        }

    }

    public void EnableArms()
    {
        foreach (var model in maleArms)
        {
            model.SetActive(true);
        }

    }

    public void EnableDefaultUpperBodyClothes()
    {
        foreach (var model in DefaultUpperBodyCloth)
        {
            model.SetActive(true);
        }

    }

    public void DisableDefaultUpperBodyClothes()
    {
        foreach (var model in DefaultUpperBodyCloth)
        {
            model.SetActive(false);
        }

    }



    public void EnableDefaultLowerBodyClothes()
    {
        foreach (var model in DefaultLowerBodyCloth)
        {
            model.SetActive(true);
        }

    }

    public void DisableDefaultLowerBodyClothes()
    {
        foreach (var model in DefaultLowerBodyCloth)
        {
            model.SetActive(false);
        }

    }


}
