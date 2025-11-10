using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class AICharacterSpawner : MonoBehaviour
{
    [Header("Character")]

    [SerializeField] GameObject characterGameObject;
    [SerializeField] GameObject instantiatedGameObject;

    private AICharacterManager aiCharacter;

    [Header("Patrol")]
    [SerializeField] bool hasPatrolPath=false;
    [SerializeField] int patrolPathID = 0;

    [Header("Manually Set Stats")]
    [SerializeField] bool manuallySetStats;
    [SerializeField] int Stamina = 150;
    [SerializeField] int health=500;


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

            if (manuallySetStats)
            {
                aiCharacter.characterStatsManager.maxHealth = health;
                aiCharacter.characterStatsManager.currentHealth = health;
                aiCharacter.characterStatsManager.maxStamina = Stamina;
                aiCharacter.characterStatsManager.currentStamina = Stamina;
            }



            aiCharacter.CreateActivationBeacon();
            aiCharacter.gameObject.SetActive(false);

            

            

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

            if (!instantiatedGameObject.activeSelf)
            {
                instantiatedGameObject.SetActive(true);
               

            }
            var agent = aiCharacter.navMeshAgent;
            if (agent != null && !agent.isOnNavMesh)
            {
                if (!agent.enabled)
                    agent.enabled = true;

                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
                    agent.Warp(hit.position);
                else
                    agent.Warp(transform.position);
            }

            aiCharacter.ResetStateMachine();

            instantiatedGameObject.SetActive(false);

        }

    }


    
}
