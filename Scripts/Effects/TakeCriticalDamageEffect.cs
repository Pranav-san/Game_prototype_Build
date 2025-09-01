using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/ Instant Effects/ Critical Damage Effect ")]
public class TakeCriticalDamageEffect : TakeDamageEffect
{


    public override void ProcessEffect(CharacterManager character)
    {
        if (character.isInvulnerable)
            return;


        base.ProcessEffect(character);

        if (character.characterStatsManager.isDead)
        {
            return;
        }
        CalculateDamage(character.characterStatsManager);
        

        

       
       

        
    }

    protected override void CalculateDamage(CharacterStatsManager character)
    {
        finalDamageDealt= Mathf.RoundToInt(physicalDamage + magicDamage + firelDamage + lightininglDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt =1;
        }
        //Debug.Log("FinalDamage: "+ finalDamageDealt); 
        //character.ConsumeHealth(finalDamageDealt);

        //character.lastAttacker = PlayerInputManager.Instance.character;

        //We Subtract Poise Damage From Characters Total
        character.totalPoiseDamage -= poiseDamage;

        float remainingPoise = character.basePoiseDefense + character.offensivePoiseBonus+ character.totalPoiseDamage;

        if (remainingPoise <= 0)
            poiseIsBroken = true;

        character.poiseResetTimer = character.defaultPoiseResetTimer;
    }



}
