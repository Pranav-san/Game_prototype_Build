using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCharacterEffectsManager : MonoBehaviour
{
   public static WorldCharacterEffectsManager instance;

    [Header("Damage")]
    public TakeDamageEffect takeDamageEffect;
    public TakeBlockedDamageEffect takeBlockedDamageEffect;

    [Header("VFX")]
    [SerializeField] public GameObject bloodSplatterVFX;

    [SerializeField] List<InstantCharacterEffect> instantEffects;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        GenerateEffectsID();
    }

    private void GenerateEffectsID()
    {
        for(int i = 0; i < instantEffects.Count; ++i)
        {
            instantEffects[i].instantEffectID = i;
        }
    }
    
}
