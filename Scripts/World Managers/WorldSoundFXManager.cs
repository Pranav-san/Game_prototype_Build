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
    public AudioClip inDoorAmbient;
    public AudioClip outDoorAmbient;
    public AudioClip pickUpItemSfx;

    [Header("UI SFX")]
    public AudioClip loadGameClickSFX;
    public AudioClip InventorySlotClickSFX;
    public AudioClip settingCategoryClickSFX;
    public AudioClip settingToggleSFX;
    public AudioClip menuOpenSFX;
    public AudioClip menuCloseSFX;
  

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

        PlayTitleScreenMusic();

        


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

    public void PlayIndoorAtmospherMusic()
    {
            audioSource.clip = inDoorAmbient;
            audioSource.loop = true;
            audioSource.Play();

    }

    public void PlayOutdoorAtmospherMusic()
    {
            audioSource.clip = outDoorAmbient;
            audioSource.loop = true;
            audioSource.Play();

    }

    public void StopOutdoorAtmospherMusic()
    {
        audioSource.clip = inDoorAmbient;
        audioSource.loop = false;
        audioSource.Stop();

    }

    public void StopIndoorAtmospherMusic()
    {
        audioSource.clip = inDoorAmbient;
        audioSource.loop = false;
        audioSource.Stop();

    }




    public void StopTitleScreenMusic()
    {
        if (titleScreenMusic != null)
        {
            audioSource.clip = titleScreenMusic; 
            audioSource.loop = true;
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

    public void PlayLoadGameClickSFX()
    {
        audioSource.PlayOneShot(loadGameClickSFX);
    }

    public void PlayInventorySlotClickSFX()
    {
        audioSource.PlayOneShot(InventorySlotClickSFX);
    }

   

    public void PlayMenuOpenSFX()
    {
        audioSource.PlayOneShot(menuOpenSFX);
    }
    public void PlayMenuCloseSFX()
    {
        audioSource.PlayOneShot(menuCloseSFX);
    }

    public void PlaySettingCategoryClickSFX()
    {
        audioSource.PlayOneShot(settingCategoryClickSFX);
    }

    public void PlaySettingToggleSFX()
    {
        audioSource.PlayOneShot(settingToggleSFX);
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
