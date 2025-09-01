using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFxManager : CharacterSoundFxManager
{
    [Header("Player Sound Fx")]
    [SerializeField] AudioClip currentFootStepAudioClipToPlay;
    playerManager player;

    protected override void Awake()
    {
        base.Awake();

        player=GetComponent<playerManager>();


    }

    public override void playBlockSoundfx()
    {

        PlaySoundfx(WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(player.playerCombatManager.currentWeaponBeingUsed.blocking));

        
    }

    public override void PlayFootstepSfx(SurfaceType surfaceType)
    {
        switch (surfaceType)
        {
            case SurfaceType.Snow:
                currentFootStepAudioClipToPlay = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.snowfootsteps);
                audioSource.PlayOneShot(currentFootStepAudioClipToPlay);
                break;

            case SurfaceType.Metal:
                currentFootStepAudioClipToPlay = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.metalfootsteps);
                audioSource.PlayOneShot(currentFootStepAudioClipToPlay);
                break;

            case SurfaceType.Wood:
                currentFootStepAudioClipToPlay = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.woodfootsteps);
                audioSource.PlayOneShot(currentFootStepAudioClipToPlay);
                break;

            case SurfaceType.Grass:
                currentFootStepAudioClipToPlay = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.grassfootsteps);
                audioSource.PlayOneShot(currentFootStepAudioClipToPlay);
                break;
            case SurfaceType.Floor:
                currentFootStepAudioClipToPlay = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.floorfootsteps);
                audioSource.PlayOneShot(currentFootStepAudioClipToPlay);
                break;

            default:
                currentFootStepAudioClipToPlay = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.defaultfootsteps);
                audioSource.PlayOneShot(currentFootStepAudioClipToPlay);

                break;

        }
    }






}
