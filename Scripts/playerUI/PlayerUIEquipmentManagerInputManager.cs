using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIEquipmentManagerInputManager : MonoBehaviour
{
  PlayerControls2 playerControls;

    [SerializeField] PlayerUIEquipmentManager playerUIEquipmentManager;

    [Header("Inputs")]
    [SerializeField] bool unEquipItemInput = false;

    private void Start()
    {
        playerUIEquipmentManager = GetComponentInParent<PlayerUIEquipmentManager>();

    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls2();

            playerControls.PlayerActions.X.performed += i => unEquipItemInput =true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandlePlayerUIEquipmentManagerInput();


    }

    private void HandlePlayerUIEquipmentManagerInput()
    {
        if (unEquipItemInput)
        {
            unEquipItemInput =false;
            playerUIEquipmentManager.UnEquipSelectedItem();
        }
    }




}
