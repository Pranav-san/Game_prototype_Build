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
    [SerializeField] GameObject[] aiCharacters;
    [SerializeField] List<GameObject> spawnedInCharacters;

    public void Update()
    {
        if (reSpawnedCharacters)
        {
            reSpawnedCharacters = false ;
            SpawnAllCharacters();
        }
        if (deSpawnedCharacters)
        {
            deSpawnedCharacters = false ;
            DeSpawnAllCharacters();
        }
    }

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
        StartCoroutine(waitForSceneToLoadThenSpawnAICharacters());
        
    }

    private IEnumerator waitForSceneToLoadThenSpawnAICharacters()
    {
        while (!SceneManager.GetActiveScene().isLoaded) 
        { 
            yield return null;
        }

        SpawnAllCharacters();

    }

    private void SpawnAllCharacters()
    {
        foreach (var character in aiCharacters)
        {
            GameObject instantiatedCharacter = Instantiate(character);
            spawnedInCharacters.Add(instantiatedCharacter);
            
        }

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
        

    }
}
