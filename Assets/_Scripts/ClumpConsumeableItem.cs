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

    public override void AttemptToConsumeItem(PlayerManager player)
    {
        base.AttemptToConsumeItem(player);
        GameObject clump = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
        //ADD HEALTH OR FP
        player.playerEffectsManager.currentParticleFX = clumpConsumeFX;
        //INSTANTIATE FLASK IN HAND AND PLAY DRINK ANIM
        player.playerEffectsManager.instantiatedFXModel = clump;
        if(curePoison)
        {
            player.playerEffectsManager.poisonBuildup = 0;
            player.playerEffectsManager.poisonAmount = player.playerEffectsManager.defaultPoisonAmount;
            player.playerEffectsManager.isPoisoned = false;

            if (player.playerEffectsManager.currentPoisonParticleFX != null)
            {
                Destroy(player.playerEffectsManager.currentPoisonParticleFX);
            }
        }
        player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
