using UnityEngine;


[CreateAssetMenu(menuName = "Items/WeaponItems/Ranged Weapons")]
public class RangedWeaponItem : WeaponItem
{
    [Header("Ranged SFX")]
    public AudioClip[] drawSound;
    public AudioClip[] releaseSound;

    
}
