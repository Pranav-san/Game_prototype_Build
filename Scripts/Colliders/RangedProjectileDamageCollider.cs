using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    [Header("Marksman")]
    public CharacterManager characterShootingProjectile;

    [Header("Collision")]
    private bool hasCollided =false;
    public Rigidbody rigidbody;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(rigidbody.angularVelocity != Vector3.zero)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.angularVelocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided)
        {
            //hasCollided=true;

            CharacterManager potentialTarget = collision.transform.GetComponent<CharacterManager>();

            if(characterShootingProjectile == null) 
                return;

            if(potentialTarget == null )
                return;

            if(WorldUtilityManager.Instance.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
            {
                DamageTarget(potentialTarget);
            }
        
            //Destroy Game Object 
            Destroy(gameObject);
           
        }
    }
}
