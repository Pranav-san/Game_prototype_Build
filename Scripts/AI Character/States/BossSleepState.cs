using UnityEngine;


[CreateAssetMenu(menuName = "A.I/States/Boss Sleep")]
public class BossSleepState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
       aiCharacter.navMeshAgent.enabled = false;

        return (this);

        
    }
}
