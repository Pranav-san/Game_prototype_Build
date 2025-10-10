using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("Debug")]
    [SerializeField] bool deSpawnedCharacters = false;
    [SerializeField] bool reSpawnedCharacters = false;

    [Header("Loading")]
    public bool isPerformingLoadingOperation = false;

    [Header("Beacon Prefab")]
    public GameObject beaconGameObject;

    [Header("Dialouge Inteactrable Prefab")]
    public GameObject dialogueInteractable;




    private Coroutine spwanAllCharactersCoroutine;
    private Coroutine deSpwanAllCharactersCoroutine;
    private Coroutine resetAllCharactersCoroutine;


    [Header("Ai Characters")]
    [SerializeField] List<AICharacterSpawner> aICharacterSpawners;

    [Header("Bosses")]
    [SerializeField] List<AIBossCharacterManager> spawnedInBosses;

    [Header("Patrol Paths")]
    [SerializeField] List<AIPatrolPath> aiPatrolPaths;


    [SerializeField] List<AICharacterManager> spawnedInCharacters;



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


    public void AddCharacterToSpwanedCharacterList(AICharacterManager character)
    {
        if (spawnedInCharacters.Contains(character))
            return;


        if (!spawnedInCharacters.Contains(character))
        {
            spawnedInCharacters.Add(character);
        }

        AIBossCharacterManager bossCharacter = character as AIBossCharacterManager;

        if (bossCharacter!=null)
        {
            if (spawnedInBosses.Contains(bossCharacter))
                return;

            spawnedInBosses.Add(bossCharacter);
        }

    }

    public void DisableAllBossFights()
    {
        for (int i = 0; i < spawnedInBosses.Count; i++)
        {
            if (spawnedInBosses[i] == null)
                continue;

            spawnedInBosses[i].BossFightIsActive = false;



        }


    }


    public void SpwanAllCharacters()
    {

        isPerformingLoadingOperation = true;

        if (spwanAllCharactersCoroutine != null)
            StopCoroutine(spwanAllCharactersCoroutine);

        spwanAllCharactersCoroutine = StartCoroutine(SpwanAllCharactersCoroutine());



    }

    private IEnumerator SpwanAllCharactersCoroutine()
    {
        for (int i = 0; i < aICharacterSpawners.Count; i++)
        {
            yield return new WaitForFixedUpdate();
            aICharacterSpawners[i].AttemptToSpwanCharacter();

            yield return null;

        }

        isPerformingLoadingOperation = false;
        yield return null;

    }

    public void DeSpawnAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (deSpwanAllCharactersCoroutine != null)
            StopCoroutine(DeSpwanAllCharactersCoroutine());

        spwanAllCharactersCoroutine = StartCoroutine(DeSpwanAllCharactersCoroutine());





    }

    private IEnumerator DeSpwanAllCharactersCoroutine()
    {
        for (int i = 0; i<spawnedInBosses.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            var boss = spawnedInBosses[i];

            if (boss != null && boss.BossFightIsActive)
            {
                boss.OnBossFightIsActiveChanged(true, false);
            }
        }
        for (int i = 0; i<spawnedInCharacters.Count; i++)
        {
            yield return new WaitForFixedUpdate();

            var character = spawnedInCharacters[i];

            if (character != null)
            {
                Destroy(character.gameObject); // Only destroy the instantiated objects, not the original assets
            }
        }

        isPerformingLoadingOperation = false;
        yield return null;

        spawnedInCharacters.Clear();

        spawnedInBosses.Clear();



    }

    public void ResetAllCharacters()
    {
        isPerformingLoadingOperation = true;

        if (resetAllCharactersCoroutine != null)
            StopCoroutine(ResetAllCharactersCoroutine());

        spwanAllCharactersCoroutine = StartCoroutine(ResetAllCharactersCoroutine());


    }

    private IEnumerator ResetAllCharactersCoroutine()
    {
        for (int i = 0; i < aICharacterSpawners.Count(); i++)
        {
            yield return new WaitForFixedUpdate();

            aICharacterSpawners[i].ResetCharacter();

            yield return null;
        }
        isPerformingLoadingOperation = false;

        yield return null;
    }



    public AIBossCharacterManager GetBossByID(int ID)
    {
        return spawnedInBosses.FirstOrDefault(boss => boss.bossID == ID);

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

        for (int i = 0; i < aiPatrolPaths.Count; i++)
        {
            if (aiPatrolPaths[i].patrolpathID == patrolPathID)
                patrolPath= aiPatrolPaths[i];
        }
        return patrolPath;

    }


}
