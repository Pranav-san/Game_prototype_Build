using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Objects")]

    [SerializeField] GameObject GameObject;
    [SerializeField] GameObject instantiatedGameObject;

    private FogWallInteractable fogWall;



    private void Start()
    {


        WorldObjectManager.instance.SpawnObject(this);
        gameObject.SetActive(false);
    }

    private void Awake()
    {


    }

    public void AttemptToSpawnerObject()
    {
        if (GameObject != null)
        {


            instantiatedGameObject = Instantiate(GameObject);

            instantiatedGameObject.transform.position = transform.position;
            instantiatedGameObject.transform.rotation = transform.rotation;

            fogWall = instantiatedGameObject.GetComponent<FogWallInteractable>();


            

            WorldObjectManager.instance.AddFogWallsToList(fogWall);

        }
    }
}
