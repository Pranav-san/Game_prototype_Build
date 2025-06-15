using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemWheelManager : MonoBehaviour
{
    
    public bool isItemWheelOpen = false;

    [Header("Wheel Slots")]
    public GameObject Slot1;
    public GameObject Slot2;
    public GameObject Slot3;
    public GameObject Slot4;
    public GameObject Slot5;
    public GameObject Slot6;

    [Header("Slot Images")]
    [SerializeField] Image Slot1_medicItems;
    [SerializeField] Image Slot2_drinkableItems;
    [SerializeField] Image Slot3_foodItems;
    [SerializeField] Image Slot4_Spell_Throwables;
    [SerializeField] Image slot5_campFire_Rest_items;
    [SerializeField] Image Slot6_lightSourceItems;

    [Header("Sub Slots")]
    [SerializeField] public GameObject medic_SubSLot;
    [SerializeField]public GameObject drinkable_SubSLot;
    [SerializeField]public GameObject foodItems_SubSLot;
    [SerializeField]public GameObject lightSource_Subslot;
    [SerializeField] public GameObject campFire_Rest_Items_SubSlot;







}
