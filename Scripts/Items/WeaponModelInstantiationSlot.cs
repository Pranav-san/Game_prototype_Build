using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelInstantiationSlot : MonoBehaviour
{
    public WeaponModelSlot weaponSlot;
    public GameObject currentWeaponModel;


    public void UnLoadWeapon()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }
    public void LoadWeaponModel(GameObject WeaponModel)
    {
        currentWeaponModel = WeaponModel;
        WeaponModel.transform.parent = transform;

        WeaponModel.transform.localPosition = Vector3.zero;
        WeaponModel.transform.localRotation = Quaternion.identity;
        WeaponModel.transform.localScale = Vector3.one;


    }

    
}
