using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
public class DrawArrowAction : ItemAction
{
    public override void PerformAction(PlayerManager player)
    {
        if (player.isInteracting)
            return;

        if (player.isHoldingArrow)
            return;

        // Animate player
        player.playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
        player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true, true);
        
        // Instantiate arrow
        GameObject loadedArrow = Instantiate(player.playerInventoryManager.currentAmmo.loadedItemModel, player.playerWeaponSlotManager.leftHandSlot.transform);
        player.playerEffectsManager.currentRangeFX = loadedArrow;

        //ANIMATE THE BOW
        Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("isDrawn", true);
        bowAnimator.Play("Bow_ONLY_Draw_01");
    }
}
