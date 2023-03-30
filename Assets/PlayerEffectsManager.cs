using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    PlayerStatsManager playerStatsManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;

    public GameObject currentParticleFX; //The particles of current effects that eeffects player
    public GameObject instantiatedFXModel;
    public int amountToBeHealed;

    private void Awake()
    {
        playerStatsManager = GetComponentInParent<PlayerStatsManager>(); //In Parent?
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
    }
    public void HealPlayerFromEffect()
    {
        playerStatsManager.HealPlayer(amountToBeHealed);
        GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
        Destroy(instantiatedFXModel.gameObject);
        playerWeaponSlotManager.LoadBothWeaponOnSlot();
    }
}
