using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterSoundFxManager : MonoBehaviour
{
     public AudioSource audioSource;

    [Header("AttackDamage FX")]
    [SerializeField]protected AudioClip[] attackDamageSFX;

    [Header("Guns")]
    public AudioClip reloadPistolSfx;
   



    protected virtual void  Start()
   {

   }
    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayRollSfx()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSfx);

    }
    public void PlaySoundfx(AudioClip soundFx, float volume =1, bool randomizePitch =true, float pitchRandom = 0.1f)
    {
        audioSource.pitch =1;

        if (randomizePitch)
        {
            audioSource.pitch +=Random.Range(-pitchRandom, pitchRandom);
        }

        audioSource.PlayOneShot(soundFx, volume);

    }

    public virtual void playBlockSoundfx()
    {

    }

    public virtual void PlayStanceBrokenSFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.stanceBreakSfx);

    }

   


    public virtual void PlayFootStepSFX(SurfaceType surfaceType)
    {
        


    }

    public virtual void PlayLadderFootStepSfx()
    {
        

    }

    public virtual void PlayRecoilSFX()
    {


    }






    public virtual void PlayCriticalStrikeSfx()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.criticalStrikeSfx);

    }

    public virtual void PlayAttackDamageSfx()
    {

        PlaySoundfx(WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(attackDamageSFX));


    }
}
