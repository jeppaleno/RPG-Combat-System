using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable/Cure Effect Clump")]
public class ClumpConsumeableItem : ConsumableItem
{
    [Header("Recovery FX")]
    public GameObject clumpConsumeFX;

    [Header("Cure FX")]
    public bool curePoison;
    //Cure Bleed
    //Cure Curse

    public override void AttemptToConsumeItem(PlayerAnimatorManager animatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(animatorManager, weaponSlotManager, playerEffectsManager);
        GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
        //ADD HEALTH OR FP
        playerEffectsManager.currentParticleFX = clumpConsumeFX;
        //INSTANTIATE FLASK IN HAND AND PLAY DRINK ANIM
        playerEffectsManager.instantiatedFXModel = clump;
        if(curePoison)
        {
            playerEffectsManager.poisonBuildup = 0;
            playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
            playerEffectsManager.isPoisoned = false;

            if (playerEffectsManager.currentPoisonParticleFX != null)
            {
                Destroy(playerEffectsManager.currentPoisonParticleFX);
            }
        }
        weaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
