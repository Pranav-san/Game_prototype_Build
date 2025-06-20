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
        gameObject.SetActive(false);
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


            if (hasPatrolPath)
            {
                aiCharacter.idle.aiPatrolPath = WorldAIManager.instance.GetAIPatrolPathByID(patrolPathID);
            }

        }
    }
}
