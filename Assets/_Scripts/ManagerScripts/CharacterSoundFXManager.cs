using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    AudioSource audioSource;
    //ATTACKING GRUNTS

    //TAKING DAMAGE GRUNTS

    //TAKING DAMAGE SOUNDS
    [Header("Taking Damage Sounds")]
    public AudioClip[] takingDamageSounds;
    private List<AudioClip> potentialDamageSounds;
    private AudioClip lastDamageSoundPlayed;

    //FOOT STEP SOUNDS

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public virtual void PlayRandomDamageSoundFX()
    {
        potentialDamageSounds = new List<AudioClip>();

        foreach (var damageSound in takingDamageSounds)
        {
            if (damageSound != lastDamageSoundPlayed)
            {
                potentialDamageSounds.Add(damageSound);
            }
        }

        int randomValue = Random.Range(0, potentialDamageSounds.Count);
        lastDamageSoundPlayed = takingDamageSounds[randomValue];
        audioSource.PlayOneShot(takingDamageSounds[randomValue], 0.4f);
    }
}
