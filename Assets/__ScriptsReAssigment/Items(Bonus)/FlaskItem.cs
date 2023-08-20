using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumable/Flask")]
public class FlaskItem : ConsumableItem
{
    [Header("Flask Type")]
    public bool estusFlask;
    public bool ashenFlask;

    [Header("Recovery Amount")]
    public int healthRecoverAmount;
    public int focusPointsRecoverAmount;

    [Header("Recovery FX")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerManager player)
    {
        base.AttemptToConsumeItem(player);
        GameObject flask = Instantiate(itemModel, player.playerWeaponSlotManager.rightHandSlot.transform);
        //ADD HEALTH OR FP
        player.playerEffectsManager.currentParticleFX = recoveryFX;
        player.playerEffectsManager.amountToBeHealed = healthRecoverAmount;
        //INSTANTIATE FLASK IN HAND AND PLAY DRINK ANIM
        player.playerEffectsManager.instantiatedFXModel = flask;

        player.playerWeaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
