using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Equipment model")]
public class EquipmentModel : ScriptableObject
{
    public EquipmentModelType equipmentModelType;
    public string maleEquipmentName;
    public string FemaleEquipmentName;





    public void LoadModel(playerManager player, bool isMale )
    {
        if (isMale)
        {
            LoadMaleModle(player);

        }
        else
        {
            LoadFemaleModel(player);
        }

    }

    private void LoadMaleModle(playerManager player)
    {
        switch(equipmentModelType)
        {
            case EquipmentModelType.FullHelmet:
                foreach(var model in player.playerEquipmentManager.maleHeadFullHelmets)
                {
                    if(model.gameObject.name == maleEquipmentName)
                    {
                        model.gameObject.SetActive(true);
                        //If you want to Assign Material Do it Here

                    }

                }
                break;
            case EquipmentModelType.Hat:
                foreach (var model in player.playerEquipmentManager.HalfHelmets)
                {
                    if (model.gameObject.name == maleEquipmentName)
                    {
                        model.gameObject.SetActive(true);
                        //If you want to Assign Material Do it Here

                    }

                }
                break;
            case EquipmentModelType.Hood:
                foreach (var model in player.playerEquipmentManager.hoods)
                {
                    if (model.gameObject.name == maleEquipmentName)
                    {
                        model.gameObject.SetActive(true);
                        //If you want to Assign Material Do it Here

                    }

                }
                break;
            case EquipmentModelType.Torso:
                foreach (var model in player.playerEquipmentManager.maleBodies)
                {
                    if (model.gameObject.name == maleEquipmentName)
                    {
                        model.gameObject.SetActive(true);
                        //If you want to Assign Material Do it Here

                    }

                }
                break;

            case EquipmentModelType.RightLeg:
                foreach (var model in player.playerEquipmentManager.maleFullLegArmor)
                {
                    if (model.gameObject.name == maleEquipmentName)
                    {
                        model.gameObject.SetActive(true);
                        //If you want to Assign Material Do it Here

                    }

                }
                break;

            case EquipmentModelType.RightUpperArm:
                foreach (var model in player.playerEquipmentManager.maleFullHandArmor)
                {
                    if (model.gameObject.name == maleEquipmentName)
                    {
                        model.gameObject.SetActive(true);
                        //If you want to Assign Material Do it Here

                    }

                }
                break;



        }

    }
    private void LoadFemaleModel(playerManager player)
    {

    }

    
}
