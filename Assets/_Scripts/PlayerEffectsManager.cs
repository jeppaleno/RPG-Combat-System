using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    PlayerManager player;

    PoisonBuildUpBar poisonBuildUpBar;
    PoisonAmountBar poisonAmountBar;

    public GameObject currentParticleFX; //The particles of current effects that eeffects player
    public int amountToBeHealed;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();

        poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
        poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
    }
    public void HealPlayerFromEffect()
    {
        player.playerStatsManager.HealCharacter(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
        Destroy(instantiatedFXModel.gameObject);
        player.playerWeaponSlotManager.LoadBothWeaponOnSlot();
    }
}
