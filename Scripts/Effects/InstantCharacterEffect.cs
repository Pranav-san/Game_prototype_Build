using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantCharacterEffect : ScriptableObject
{
    //Base Class For All Instant Effects

    [Header("Effect ID")]
    public int instantEffectID;

    public virtual void ProcessEffect(CharacterManager character)
    {


    }
}
