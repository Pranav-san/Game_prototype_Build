using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedProjectileDamageCollider : DamageCollider
{
    [Header("Marksman")]
    public CharacterManager characterShootingProjectile;

    [Header("Collision")]

    private bool hasPenetratedSurfate;
    public Rigidbody rigidbody;
    CapsuleCollider capsuleCollider;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        if (rigidbody.angularVelocity != Vector3.zero)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.angularVelocity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreatePenetrationIntoObkect(collision);

        CharacterManager potentialTarget = collision.transform.GetComponent<CharacterManager>();

        if (characterShootingProjectile == null)
            return;

        if (potentialTarget == null)
            return;

        if (WorldUtilityManager.Instance.CanIDamageThisTarget(characterShootingProjectile.characterGroup, potentialTarget.characterGroup))
        {
            DamageTarget(potentialTarget);
        }

        //Destroy Game Object 
        Destroy(gameObject);


    }


    private void CreatePenetrationIntoObkect(Collision hit)
    {
        if (!hasPenetratedSurfate)
        {
            hasPenetratedSurfate = true;

            //Get Contact Point
            gameObject.transform.position = hit.GetContact(0).point;


            var emptyGameObject = new GameObject();
            emptyGameObject.transform.parent = hit.collider.transform;
            gameObject.transform.SetParent(emptyGameObject.transform, true);

            //How Far The Arrow Penetrate Through Surface
            transform.position+= transform.forward * Random.Range(0.1f, 0.2f);


            //Disable Colliders and Rigidbody
            rigidbody.isKinematic = true;
            capsuleCollider.enabled = false;
            
            //Destroy Damage Collider and Projectile After A Time
            Destroy(GetComponent<RangedProjectileDamageCollider>());
            Destroy(gameObject, 7);





        }


    }
}
