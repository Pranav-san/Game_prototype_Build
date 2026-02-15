using UnityEngine;

public class EventTriggerAtmosphere : MonoBehaviour
{
    public LocationType locationType;

    private void Awake()
    {
        //collider = GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        playerManager player = other.GetComponent<playerManager>();

        if(player != null )
        {
            if (player.playerSoundFxManager.locationType != locationType)
            {
                player.playerSoundFxManager.locationType = locationType;
                player.playerSoundFxManager.SwitchAtmosphereSFXBasedOnLocationType();

            }
            
            
        }
    }

}
