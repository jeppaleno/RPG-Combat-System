using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;

    PoisonBuildUpBar poisonBuildUpBar;
    PoisonAmountBar poisonAmountBar;

    public GameObject currentParticleFX; //The particles of current effects that eeffects player
    public GameObject instantiatedFXModel;
    public int amountToBeHealed;

    protected override void Awake()
    {
        base.Awake();
        playerStatsManager = GetComponentInParent<PlayerStatsManager>(); //In Parent?
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

        poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
        poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
    }
    public void HealPlayerFromEffect()
    {
        playerStatsManager.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
        Destroy(instantiatedFXModel.gameObject);
        playerWeaponSlotManager.LoadBothWeaponOnSlot();
    }

    protected override void HandlePoisonBuildUp()
    {
        if (poisonBuildup <= 0)
        {
            poisonBuildUpBar.gameObject.SetActive(false);
        }
        else
        {
            poisonBuildUpBar.gameObject.SetActive(true);
        }
        base.HandlePoisonBuildUp();
        poisonBuildUpBar.SetPoisonBuildUpAmount(Mathf.RoundToInt(poisonBuildup));
    }

    protected override void HandleIsPoisonedEffect()
    {
        if (isPoisoned == false)
        {
            poisonAmountBar.gameObject.SetActive(false);
        }
        else
        {
            poisonAmountBar.gameObject.SetActive(true);
        }

        base.HandleIsPoisonedEffect();
        poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
    }
}
