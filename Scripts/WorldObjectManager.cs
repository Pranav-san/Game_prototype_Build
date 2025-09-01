using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WorldObjectManager : MonoBehaviour
{
    public static WorldObjectManager instance;


    [Header("Sites Of Grace")]
    public List<SiteOfGrace> sitesOfGrace;

    [Header("Fog Walls")]
    public List<FogWallInteractable> fogWalls;

    [Header("object")]
    [SerializeField] List<ObjectSpawner> objectSpawners;
    [SerializeField] List<GameObject> spwanedInObjects;

    //[Header("Debug")]
    //[SerializeField] bool deSpawnedCharacters = false;
    //[SerializeField] bool reSpawnedCharacters = false;








    private void Awake()
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



    public void SpawnObject(ObjectSpawner objectSpawner)
    {
        objectSpawners.Add(objectSpawner);
        objectSpawner.AttemptToSpawnerObject();

    }


   
    public void AddSiteOfGraceToTheList(SiteOfGrace siteOfGrace)
    {
        if (!sitesOfGrace.Contains(siteOfGrace))
        {
            sitesOfGrace.Add(siteOfGrace);
        }

    }

   

    public void RemoveSiteOfGraceToTheList(SiteOfGrace siteOfGrace)
    {
        if (sitesOfGrace.Contains(siteOfGrace))
        {
            sitesOfGrace.Remove(siteOfGrace);
        }

    }

    public void AddFogWallsToList(FogWallInteractable fogWall)
    {
        if (!fogWalls.Contains(fogWall))
        {
            fogWalls.Add(fogWall);

        }

    }

    public void RemoveFogWallsFromList(FogWallInteractable fogWall)
    {
        if (fogWalls.Contains(fogWall))
        {
            fogWalls.Remove(fogWall);

        }


    }
}
