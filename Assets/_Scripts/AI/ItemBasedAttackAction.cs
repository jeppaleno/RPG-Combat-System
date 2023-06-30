using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Actions")]
public class ItemBasedAttackAction : ScriptableObject
{
    [Header("Attack Type")]
    public AIAttackActionType actionAttackType = AIAttackActionType.meleeAttackAction;
    public AttackType attackType = AttackType.light;

    [Header("Action Combo Settings")]
    public bool actionCanCombo = false;

    [Header("Right Hand Or Left Hand Action")]
    bool isUsingRightHandedAction = true;

    [Header("Action Settings")]
    public int attackScore = 3;
    public float recoveryTime = 2;
    public float maximumAttackAngle = 35;
    public float minimumAttackAngle = -35;
    public float minimumDistanceNeededToAttack = 0;
    public float maximumDistanceNeededToAttack = 1.5f;

    public void PerformAttackAction(AICharacterManager enemy)
    {
        if (isUsingRightHandedAction)
        {
            enemy.UpdateWhichHandCharacterIsUsing(true);
            PerformRightHandItemActionBasedOnAttackType(enemy);
        }
        else
        {
            enemy.UpdateWhichHandCharacterIsUsing(false);
            PerformLeftHandItemActionBasedOnAttackType(enemy);
        }
    }

    //DECIDE WHICH HAND PERFORMS ACTIONS
    private void PerformRightHandItemActionBasedOnAttackType(AICharacterManager enemy)
    {
        if (actionAttackType == AIAttackActionType.meleeAttackAction)
        {
            PerformRightHandMeleeAction(enemy);
        }
        else if (actionAttackType != AIAttackActionType.rangedAttackAction)
        {
            //perform right hand ranged action
        }
    }

    private void PerformLeftHandItemActionBasedOnAttackType(AICharacterManager enemy)
    {
        if (actionAttackType == AIAttackActionType.meleeAttackAction)
        {
            //Perform left hand melee action
        }
        else if (actionAttackType != AIAttackActionType.rangedAttackAction)
        {
            //perform left hand ranged action
        }
    }

    //RIGHT HAND ACTIONS
    private void PerformRightHandMeleeAction(AICharacterManager enemy)
    {
        if (enemy.isTwoHandingWeapon)
        {
            if (attackType == AttackType.light)
            {
                enemy.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemy);
            }
            else if (attackType == AttackType.heavy)
            {
                enemy.characterInventoryManager.rightWeapon.th_tap_RT_Action.PerformAction(enemy);
            }
        }
        else
        {
            if (attackType == AttackType.light)
            {
                enemy.characterInventoryManager.rightWeapon.oh_tap_RB_Action.PerformAction(enemy);
            }
            else if (attackType == AttackType.heavy)
            {
                enemy.characterInventoryManager.rightWeapon.oh_tap_RT_Action.PerformAction(enemy);
            }
        }
    }
}
