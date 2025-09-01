using UnityEngine;

public class FogWallInteractable : Interactable
{


    [Header("Fog")]
    [SerializeField] GameObject[] fogGameObjects;

    [Header("ID")]
    public int fogWallID;

    [Header("Active")]
    [SerializeField] bool isActive = true;

    protected override void Start()
    {
        base.Start();

        WorldObjectManager.instance.AddFogWallsToList(this);
    }

    public override void Interact(playerManager player)
    {
        base.Interact(player);

        Quaternion targetRotation = Quaternion.LookRotation(-Vector3.forward);
        player.transform.rotation = targetRotation;


    }



    private void OnIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if (isActive)
        {
            foreach (var fogObject in fogGameObjects)
                fogObject.SetActive(true);
        }
        else
        {
            foreach (var fogObject in fogGameObjects)
                fogObject.SetActive(false);
        }
    }

    public void SetActive(bool value)
    {
        isActive = value;

        foreach (var fogObject in fogGameObjects)
        {
            fogObject.SetActive(value);
        }
    }




}

