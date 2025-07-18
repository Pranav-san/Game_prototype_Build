using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFxManager : MonoBehaviour
{
     public AudioSource audioSource;

    [Header("AttackDamage FX")]
    [SerializeField]protected AudioClip[] attackDamageSFX;

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
        audioSource.PlayOneShot(soundFx, volume);

        audioSource.pitch =1;

        if (randomizePitch)
        {
            audioSource.pitch +=Random.Range(-pitchRandom, pitchRandom);
        }

    }

    public virtual void playBlockSoundfx()
    {

    }

    public virtual void PlayAttackDamageSfx()
    {

        PlaySoundfx(WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(attackDamageSFX));


    }
}
