using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUtilityManager : MonoBehaviour
{
  public static WorldUtilityManager Instance;

    [Header("Layers")]
    [SerializeField] LayerMask characterLayers;
    [SerializeField] LayerMask enviroLayers;




    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LayerMask GetCharacterLayer()
    {
        return characterLayers;

    }
    public LayerMask GetEnviroLayer()
    {
        return enviroLayers;
    }

    public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
    {
        if (attackingCharacter==CharacterGroup.Team01)
        {
            switch(targetCharacter)
            {
                case CharacterGroup.Team01:return false;
                case CharacterGroup.Team02:return true;
                    
                default:
                    break;
            }
        }
        else if(attackingCharacter == CharacterGroup.Team02)
        {
            switch (targetCharacter)
            {
                case CharacterGroup.Team01: return true;    
                case CharacterGroup.Team02: return false;
                    
                default:
                    break;
            }

        }
        return false;

    }

    public float GetAngleOfTarget(Transform characterTransform, Vector3 targetDirecton)
    {
        targetDirecton.y=0;
        float viewableAngle = Vector3.Angle(characterTransform.forward, targetDirecton);
        Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirecton);

        if (cross.y<0)
        {
            viewableAngle = -viewableAngle;
        }
        return viewableAngle;
    }

    public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
    {
        //Throwing Daggers, stones
        DamageIntensity damageIntensity = DamageIntensity.Ping;

        //
        if(poiseDamage >=25)
            damageIntensity = DamageIntensity.Light;

        if(poiseDamage>=60)
            damageIntensity = DamageIntensity.Medium;

        if(poiseDamage >=70) 
            damageIntensity = DamageIntensity.Heavy;

        if(poiseDamage >= 120)
            damageIntensity = DamageIntensity.Collasal;

        return damageIntensity;



    }
}
