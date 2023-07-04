using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireInteractable : Interactable
{
    //Loaction of bonfire (for teleporting)

    //Bonfire unique ID (for saving)

    [Header("Bonfire Teleport Transform")]
    public Transform bonfireTeleportTransform;

    [Header("Activation Status")]
    public bool hasBeenActivated;

    [Header("Bonfire FX")]
    public ParticleSystem activationFX;
    public ParticleSystem fireFX;
    public AudioClip bonfireActivationSoundFX;

    AudioSource audioSource;

    private void Awake()
    {
        //if the Bonfire has already been activated by the player, play the Fire FX when the bonfire is loaded into the scene
        if (hasBeenActivated)
        {
            fireFX.gameObject.SetActive(true);
            fireFX.Play();
            interactbleText = "Rest";
        }
        else
        {
            interactbleText = "Light Bonfire";
        }

        audioSource = GetComponent<AudioSource>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        Debug.Log("You Interacted w bonfire");

        if (hasBeenActivated)
        {
            //Open the teleport menu
        }
        else
        {
            //Activate Bonfire
            playerManager.playerAnimatorManager.PlayTargetAnimation("Bonfire_Activate", true, true);
            playerManager.uiManager.ActivateBonfirePopUp();
            hasBeenActivated = true;
            interactbleText = "Rest";
            activationFX.gameObject.SetActive(true);
            activationFX.Play();
            fireFX.gameObject.SetActive(true);
            fireFX.Play();
            audioSource.PlayOneShot(bonfireActivationSoundFX);
        }
    }
}
