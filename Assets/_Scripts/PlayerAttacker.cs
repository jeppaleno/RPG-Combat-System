using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorManager animatorHandler;
    InputManager inputManager;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if(inputManager.comboFlag)
        {
            animatorHandler.animator.SetBool("canDoCombo", false);

            if (lastAttack == weapon.OH_Light_Attack_1)
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true, true); // Attack with root motion
            }
            else if (lastAttack == weapon.th_light_attack_01)
            {
                animatorHandler.PlayTargetAnimation(weapon.th_light_attack_02, true, true);
            }
        }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twohandFlag)
        {
            animatorHandler.PlayTargetAnimation(weapon.th_light_attack_01, true, true);
            lastAttack = weapon.th_light_attack_01;
        }
        else
        {
            
            animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true, true); // attack with root motion
            lastAttack = weapon.OH_Light_Attack_1;
        }
        
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        weaponSlotManager.attackingWeapon = weapon;

        if (inputManager.twohandFlag)
        {

        }
        else
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true, true); // Attack with root motion
            lastAttack = weapon.OH_Light_Attack_1;
        }
        
    }
}
