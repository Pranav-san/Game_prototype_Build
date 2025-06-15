using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
   public static WorldAIManager instance;

    [Header("Debug")]
    [SerializeField] bool deSpawnedCharacters = false;
    [SerializeField] bool reSpawnedCharacters = false;


    [Header("Ai Characters")]
    [SerializeField] List<AICharacterSpawner> aICharacterSpawners;

    [Header("Patrol Paths")]
    [SerializeField] List<AIPatrolPath> aiPatrolPaths;

    
    [SerializeField] List<GameObject> spawnedInCharacters;

    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        
        
    }

   

    public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
    {
        aICharacterSpawners.Add(aiCharacterSpawner);
        aiCharacterSpawner.AttemptToSpwanCharacter();

    }
    public void DeSpawnAllCharacters()
    {
        foreach (var character in spawnedInCharacters)
        {
            if (character != null)
            {
                Destroy(character); // Only destroy the instantiated objects, not the original assets
            }

            
        }
        spawnedInCharacters.Clear();


    }

    public void ResetAllCharacters()
    {
        DeSpawnAllCharacters();

        reSpawnedCharacters = true;
       

       

        

    }


    //Patrol Paths
    public void AddPatrolPathToList(AIPatrolPath patrolPath)
    {
        if (aiPatrolPaths.Contains(patrolPath))
        return;

        aiPatrolPaths.Add(patrolPath);
        
    }

    public AIPatrolPath GetAIPatrolPathByID(int patrolPathID)
    {
        AIPatrolPath patrolPath = null;

        for (int i = 0; i < aiPatrolPaths.Count; i++ )
        {
            if (aiPatrolPaths[i].patrolpathID == patrolPathID)
            patrolPath= aiPatrolPaths[i];
        }
        return patrolPath;

    }
}
