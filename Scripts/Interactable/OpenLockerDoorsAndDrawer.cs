using UnityEngine;

public class OpenLockerDoorsAndDrawer : Interactable 
{
    [Header("Settings")]
    public Transform LockerDoor;
    private bool isdoorOpen;
    public Vector3 openRotation;
    public Vector3 closeRotation;
    

    protected override void Start()
    {
        if (LockerDoor == null)
            LockerDoor = transform;


    }




    public override void Interact(playerManager player)
    {
        if (!isdoorOpen)
        {
            isdoorOpen = true;
            LockerDoor.localRotation = Quaternion.Euler(openRotation);
        }
        else
        {
            isdoorOpen = false;
            LockerDoor.localRotation = Quaternion.Euler(closeRotation);

        }

       
        
    }


}
