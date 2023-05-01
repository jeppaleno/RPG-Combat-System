using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Draw Arrow Action")]
public class DrawArrowAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.isInteracting)
            return;

        if (character.isHoldingArrow)
            return;

        // Animate player
        character.animator.SetBool("isHoldingArrow", true);
        character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true, true);
        
        // Instantiate arrow
        GameObject loadedArrow = Instantiate(character.characterInventoryManager.currentAmmo.loadedItemModel, character.characterWeaponSlotManager.leftHandSlot.transform);
        character.characterEffectsManager.currentRangeFX = loadedArrow;

        //ANIMATE THE BOW
        Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("isDrawn", true);
        bowAnimator.Play("Bow_ONLY_Draw_01");
    }
}
