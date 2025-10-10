using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    public AudioSource audioSource;


    [Header("Damage Sounds")]
    public AudioClip[] unarmedDamageSFX;
    public AudioClip[] weaponDamageSFX;
    public AudioClip[] greatDamageSFX;
    public AudioClip[] physicalDamageSFX;

    




    [Header("World Bg Music")]
    public AudioClip bgMusic;
    public AudioClip stormSfx;
    public AudioClip pickUpItemSfx;
  

    [Header("Action Sound Fx")]
    public AudioClip rollSfx;
    public AudioClip stanceBreakSfx;
    public AudioClip criticalStrikeSfx;

    [Header("Foot Steps")]
    public AudioClip[] defaultfootsteps;
    public AudioClip[] snowfootsteps;
    public AudioClip[] woodfootsteps;
    public AudioClip[] metalfootsteps;
    public AudioClip[] floorfootsteps;
    public AudioClip[] grassfootsteps;
    public AudioClip[] ladderfootsteps;


    [Header("Door")]
    public AudioClip doorPassCodeButtonPressetSfx;
    public AudioClip wrongPassCodeSfx;
    public AudioClip correctPassCodeSfx;

    public AudioClip useKeySfx;


    [Header("Menu Music")]
    public AudioClip titleScreenMusic; 







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

        


    }

    

   

    public void PlayTitleScreenMusic()
    {
        if (titleScreenMusic != null)
        {
            audioSource.clip = titleScreenMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    public void StopTitleScreenMusic()
    {
        if (titleScreenMusic != null)
        {
            audioSource.clip = titleScreenMusic; 
            audioSource.loop = true; // Set it to loop if the storm effect needs to play continuously
            audioSource.Stop(); 
        }

    }

    public void playDoorUnlockButtonPressed()
    {
        audioSource.PlayOneShot(doorPassCodeButtonPressetSfx);
    }

    public void PlayWrongPasscodeSfx()
    {
        audioSource.PlayOneShot(wrongPassCodeSfx);
    }

    public void PlayCorrectPasscodeSfx()
    {
        audioSource.PlayOneShot(correctPassCodeSfx);
    }

    public void PlayUseKeySfx()
    {
        audioSource.PlayOneShot(useKeySfx);
    }
    public AudioClip ChooseRandomSoundFxFromArray(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }

    public void AlertNearByCharactersToSound(Vector3 positionOfSound,  float rangeOfSound)
    {
        Collider[] characterColliders =  Physics.OverlapSphere(positionOfSound, rangeOfSound, WorldUtilityManager.Instance.GetCharacterLayer());

        List<AICharacterManager> charactersToAlert = new List<AICharacterManager>();

        for (int i = 0; i < characterColliders.Length; i++)
        {
            AICharacterManager aiCharacter = characterColliders[i].GetComponent<AICharacterManager>();

            if(aiCharacter == null)
                continue;
            if(charactersToAlert.Contains(aiCharacter))
                continue;

            charactersToAlert.Add(aiCharacter);
        }

        for (int i = 0;i < charactersToAlert.Count; i++)
        {
            charactersToAlert[i].aiCharacterCombatManager.AlertCharacterToSound(positionOfSound);
        }
    }


}
