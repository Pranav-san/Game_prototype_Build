using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    public AudioSource audioSource;

    




    [Header("World Bg Music")]
    public AudioClip bgMusic;
    public AudioClip stormSfx;
    public AudioClip pickUpItemSfx;
  

    [Header("Action Sound Fx")]
    public AudioClip rollSfx;
    public AudioClip footstepSfx;




    private void Awake()
    {
        if(instance == null)
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
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        PlayStormSfx();
    }

    

    public void PlayFootstep()
    {
        if (footstepSfx != null)
        {
            audioSource.PlayOneShot(footstepSfx);
        }
    }


    public void PlayStormSfx()
    {
        if (stormSfx != null)
        {
            audioSource.clip = stormSfx; // Assign the storm sound effect
            audioSource.loop = true; // Set it to loop if the storm effect needs to play continuously
            audioSource.Play(); // Play the storm SFX
        }

    }

    public AudioClip ChooseRandomSoundFxFromArray(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }


}
