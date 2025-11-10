using UnityEngine;

public class AICharacterEffectsManager : CharacterEffectsManager
{

    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AICharacterManager>();
    }
    
       
    
    public override void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        base.ProcessInstantEffect(effect);

        if(aiCharacter == null ) 
            return;

        if(aiCharacter.characterStatsManager.isDead)
            return;

        if (aiCharacter.characterStatsManager.lastAttacker == null)
            return;


        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
            aiCharacter.aiCharacterCombatManager.currentTarget = aiCharacter.characterStatsManager.lastAttacker;

        }

        
    }



}
