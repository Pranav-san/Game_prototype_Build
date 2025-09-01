using UnityEngine;


[CreateAssetMenu(menuName ="Items/Ranged Projectile")]
public class RangedProjectileItem : Item
{
    public ProjectileClass projectileClass;

    [Header("Velocity")]
    public float forwardVelocity = 2200;
    public float upwardVelocity = 0;
    public float AmmoMass = 0.01f;

    [Header("Capacity")]
    public int currentAmmoAmount = 30;
    public int pickUpAmount = 3;

    [Header("Magazine")]
    public int magazineSize = 6;// per reload capacity
    public int currentMagazineAmmo = 6;



    [Header("Damage")]
    public int physicalDamage = 0;

    [Header("Model")]
    public GameObject drawProjectileModel;
    public GameObject releaseProjectileModel;



}
