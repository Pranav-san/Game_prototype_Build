using UnityEngine;

public class AICharacterInventoryManager : CharacterInventoryManager
{
    [Header("Projectiles")]
    public RangedProjectileItem mainProjectile;
    public RangedProjectileItem secondaryProjectile;
    public bool hasArrowNotched = false;
    public bool isHoldingArrow = false;
    public bool isAiming = false;
    public bool FireBullet = false;



}
