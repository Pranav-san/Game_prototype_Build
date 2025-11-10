using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyManager : MonoBehaviour
{
    //Enable Body Parts

    

    [Header("Hair game object")]
    [SerializeField] public GameObject hair;
    [SerializeField] public GameObject[] hairObjects;
    [SerializeField] public GameObject facialHair;

    [Header("Male")]
    public GameObject maleObject;
    [SerializeField] public GameObject maleHead;  //Default Head Model when Unequipping Armor or Outfit
    [SerializeField] public GameObject[] maleBody;//Default UpperBodyModel When Unequipping Armor or Outfit
    [SerializeField] public GameObject[] maleUpperArms;//Default UpperBodyModel When Unequipping Armor or Outfit(Lower left arm, Left Hand, Lower Right Arm, Right Hand)
    [SerializeField] public GameObject[] maleLowerArms;//Default UpperBodyModel When Unequipping Armor or Outfit(Lower left arm, Left Hand, Lower Right Arm, Right Hand)
    [SerializeField] public GameObject[] maleLegs;//Default Hips, Left and Right Leg When Unequipping Armor or Outfit
    [SerializeField] public GameObject[] DefaultUpperBodyCloth; //Vest...Etc
    [SerializeField] public GameObject[] DefaultLowerBodyCloth;//LoinCloth, Briefs..Etc

    [SerializeField] public GameObject maleEyes;
    [SerializeField] public GameObject[] maleFacialHair;

    

    [Header("Female")]
    public GameObject feMaleObject;
    [SerializeField] public GameObject femaleHead;
    [SerializeField] public GameObject[] femaleBody;
    [SerializeField] public GameObject[] femaleUpperArms;
    [SerializeField] public GameObject[] femaleLowerArms;
    [SerializeField] public GameObject[] femaleLegs;
    [SerializeField] public GameObject femaleEyes;


    [Header("Mesh Render")]
    public SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
       
    }



    //Dead Spot
   

    //Equipment
    public void EnableHead()
    {
        //Enable Head Object
        maleHead.SetActive(true);
        //femaleHead.SetActive(true);

        //Enable Facial Features
        //maleEyes.SetActive(true);
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
        //hair.SetActive(true);
    }
    public void DisableHair()
    {
        if(hair!=null)
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

        foreach (var model in femaleBody)
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

        foreach (var model in femaleBody)
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



        foreach (var model in femaleLegs)
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

        foreach (var model in femaleLegs)
        {
            model.SetActive(false);
        }



    }

    public void EnableUpperArms()
    {
        foreach (var model in maleUpperArms)
        {
            model.SetActive(true);
        }

        foreach (var model in femaleUpperArms)
        {
            model.SetActive(true);
        }

    }

    public void DisableUpperArms()
    {
        foreach (var model in maleUpperArms)
        {
            model.SetActive(false);
        }

        foreach (var model in femaleUpperArms)
        {
            model.SetActive(false);
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

    public void ToggleBodyType(bool isMale)
    {
        if (isMale)
        {
            maleObject.SetActive(true);
            feMaleObject.SetActive(false);

        }
        else
        {
            maleObject.SetActive(false);
            feMaleObject.SetActive(true);
        }
    }

    public void ToggleHairType(int hairType)
    {

        //Disable All Hair Objects
        for (int i = 0; i < hairObjects.Length; i++)
        {
            hairObjects[i].SetActive(false);

        }

        //Enable Choosen Hair
        hairObjects[hairType].SetActive(true);


    }

    public void TogglePlayerObject(bool status)
    {
        if (status)
        {
            for(int i = 0;meshRenderers.Length > 0; i++)
            {
                meshRenderers[i].enabled =true;
            }
        }
        else
        {
            for (int i = 0; meshRenderers.Length > 0; i++)
            {
                meshRenderers[i].enabled =false;
            }

        }
    }





}
