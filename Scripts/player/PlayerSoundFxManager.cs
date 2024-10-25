using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFxManager : CharacterSoundFxManager
{
    [Header("Player Sound Fx")]
    [SerializeField] AudioClip footSteps;

    public void playeFootSteps()
    {
        audioSource.PlayOneShot(footSteps);
    }





}
