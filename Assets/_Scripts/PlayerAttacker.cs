using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorManager;
    InputManager inputManager;
    public string lastAttack;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if(inputManager.comboFlag)
        {
            animatorManager.animator.SetBool("canDoCombo", false);
            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
        lastAttack = weapon.OH_Light_Attack_1;
    }
}
