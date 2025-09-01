using UnityEngine;

public class ExploderDamageCollider : DamageCollider
{

    AICharacterManager characterCausingDamage;
    public GameObject explosionVfx;


    protected override void Awake()
    {
        characterCausingDamage = GetComponentInParent<AICharacterManager>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget == null || damageTarget == characterCausingDamage)
            return;


        contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);





        damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Heavy impact", true);




        CheckForBlock(damageTarget);
        DamageTarget(damageTarget);
        if (explosionVfx != null)
        {
            explosionVfx.SetActive(true);

        }

        characterCausingDamage.hasExploded = true;






    }









}
