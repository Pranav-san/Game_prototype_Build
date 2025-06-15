using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFxManager : CharacterSoundFxManager
{
    [Header("Player Sound Fx")]
    [SerializeField] AudioClip footSteps;

    playerManager player;

    public void playeFootSteps()
    {
        audioSource.PlayOneShot(footSteps);
    }

    protected override void Awake()
    {
        base.Awake();

        player=GetComponent<playerManager>();


    }

    public override void playBlockSoundfx()
    {

        PlaySoundfx(WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(player.playerCombatManager.currentWeaponBeingUsed.blocking));

        
    }






}
