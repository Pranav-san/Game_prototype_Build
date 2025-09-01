using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILockOn : MonoBehaviour
{
    [SerializeField] GameObject lockoff;
    [SerializeField]GameObject lockon;

   

    public void Update()
    {
        EnablelockOn();
    }

    public void EnablelockOn()
    {
       

            if (playerManager.instance.playerCombatManager.isLockedOn)
            {
                lockoff.SetActive(false);
                lockon.SetActive(true);



            }
            else
            {
                lockoff.SetActive(true);
                lockon.SetActive(false);

            }
        
       
       
    }
    
}
