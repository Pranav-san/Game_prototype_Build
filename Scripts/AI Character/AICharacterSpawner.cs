using UnityEngine;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]

    [SerializeField] GameObject characterGameObject;
    [SerializeField] GameObject instantiatedGameObject;

    private AICharacterManager aiCharacter;

    [Header("Patrol")]
    [SerializeField] bool hasPatrolPath=false;
    [SerializeField] int patrolPathID = 0;

    private void Start()
    {
        

        WorldAIManager.instance.SpawnCharacter(this);
        //gameObject.SetActive(false);
    }

    private void Awake()
    {

        

    }

    public void AttemptToSpwanCharacter()
    {
        if (characterGameObject != null)
        {
            

            instantiatedGameObject = Instantiate(characterGameObject);

            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;

            aiCharacter = instantiatedGameObject.GetComponent<AICharacterManager>();


            if (aiCharacter!=null)
            {
                WorldAIManager.instance.AddCharacterToSpwanedCharacterList(aiCharacter);

            }


            if (hasPatrolPath)
            {
                aiCharacter.idle.aiPatrolPath = WorldAIManager.instance.GetAIPatrolPathByID(patrolPathID);
            }

            

            

        }
    }

    public void ResetCharacter()
    {
        if(instantiatedGameObject == null)
            return;
        if(aiCharacter == null)
            return;

        if (characterGameObject != null)
        {
            aiCharacter.characterController.enabled = true;
            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;
            aiCharacter.characterStatsManager.currentHealth = aiCharacter.characterStatsManager.maxHealth;
            aiCharacter.aiCharacterCombatManager.currentStance = aiCharacter.aiCharacterCombatManager.maxStance;
            aiCharacter.aiCharacterCombatManager.stanceRegenerationTimer = 0f;
            aiCharacter.aiCharacterCombatManager.currentTarget = null;
            aiCharacter.ResetStateMachine();


            if (aiCharacter.characterStatsManager.isDead)
            {
                aiCharacter.characterStatsManager.isDead = false;
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Empty", false, false, true, true);
                aiCharacter.isHoldingArrow = false;
                aiCharacter.ResetStateMachine();
                aiCharacter.hasExploded = false;

                



            }
           


        }

    }
}
