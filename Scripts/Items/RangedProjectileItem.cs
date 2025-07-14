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
    public int capacity = 30;
    public int currentAmmoAmount = 30;

    [Header("Damage")]
    public int physicalDamage = 0;

    [Header("Model")]
    public GameObject drawProjectileModel;
    public GameObject releaseProjectileModel;



}
