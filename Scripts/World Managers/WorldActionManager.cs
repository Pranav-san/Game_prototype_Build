using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldActionManager : MonoBehaviour
{
   public static WorldActionManager Instance;

    [Header("weapon Item Action")]
    public WeaponItemBasedAction[] weaponItemActions;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        for(int i = 0; i<weaponItemActions.Length; i++)
        {
            weaponItemActions[i].actionId = i;
        }
    }

    public WeaponItemBasedAction GetWeaponItemActionByID(int ID)
    {
        return weaponItemActions.FirstOrDefault(action => action.actionId == ID);

    }
}
