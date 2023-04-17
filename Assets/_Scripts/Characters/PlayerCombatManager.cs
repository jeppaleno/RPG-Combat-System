using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    CameraManager cameraManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerEquipmentManager playerEquipmentManager;
    PlayerManager playerManager;
    PlayerStatsManager playerStatsManager;
    PlayerInventoryManager playerInventoryManager;
    InputManager inputManager;
    PlayerWeaponSlotManager playerWeaponSlotManager;
    PlayerEffectsManager playerEffectsManager;

    [Header("Attack Animations")]
    public string oh_light_attack_01 = "OH_Light_Attack_01";
    public string oh_light_attack_02 = "OH_Light_Attack_02";
    public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
    public string oh_heavy_attack_02 = "OH_Heavy_Attack_02"; //ADD LATER
    public string oh_running_attack_01 = "OH_Running_Attack_01";
    public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";

    public string th_light_attack_01 = "TH_Light_Attack_01";
    public string th_light_attack_02 = "TH_Light_Attack_02";
    public string th_heavy_attack_01 = "TH_Heavy_Attack_01"; //ADD LATER
    public string th_heavy_attack_02 = "TH_Heavy_Attack_02"; //ADD LATER
    public string th_running_attack_01 = "TH_Running_Attack_01";
    public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

    public string weapon_art = "Weapon_Art";

    public string lastAttack;

    LayerMask backStabLayer = 1 << 12;
    LayerMask riposteLayer = 1 << 13;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerManager = GetComponent<PlayerManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        inputManager = GetComponent<InputManager>();
    }

    public void HandleHoldRBAction()
    {
        if (playerManager.isTwoHandingWeapon)
        {
            PerformRBRangedAction();
        }
        else
        {
            //Do a melee attack (Bow Bash...)
        }
    }

    public void HandleRBAction()
    {
        PerformMagicAction(playerInventoryManager.rightWeapon, true); //HUHUHHHHHHHHHHHHHHHH false instead?
    }

    public void HandleLBAction()
    {
        if (playerManager.isTwoHandingWeapon)
        {
            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
            {
                PerformLBAimingAction();
            }
        }
        else
        {
            if (playerInventoryManager.leftWeapon.weaponType == WeaponType.shield ||
                playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
            {
                performLBBlockingAction();
            }
            else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster ||
                playerInventoryManager.leftWeapon.weaponType == WeaponType.PyromancyCaster)
            {
                PerformMagicAction(playerInventoryManager.leftWeapon, true);
                playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
            }
        }
    }

    public void HandleLTAction()
    {
        if (playerInventoryManager.leftWeapon.weaponType == WeaponType.shield
            || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
        {
            PerformLTWeaponArt(inputManager.twohandFlag);
        }
        else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
        {
            //do a light attack
        }
    }

    private void DrawArrowAction()
    {
        playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
        playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true, true);
        GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManager.leftHandSlot.transform);
        //ANIMATE THE BOW
        Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("isDrawn", true);
        bowAnimator.Play("Bow_ONLY_Draw_01");
        playerEffectsManager.currentRangeFX = loadedArrow;
    }

    public void FireArrowAction()
    {
        //Create the Live arrow instantiation location
        ArrowInstantiationLocation arrowInstantiationLocation;
        arrowInstantiationLocation = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

        //animate the bow firing the arrow
        Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("isDrawn", false);
        bowAnimator.Play("Bow_ONLY_Fire_01");
        Destroy(playerEffectsManager.currentRangeFX); //Destroys loaded arrow model

        //Reset the players holding arrow flag
        playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true, true);
        playerAnimatorManager.animator.SetBool("isHoldingArrow", false);

        //Create and fire the live arrow
        GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, cameraManager.cameraPivot.rotation);
        Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

        if (playerManager.isAiming)
        {
            Ray ray = cameraManager.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint, 100.0f))
            {
                liveArrow.transform.LookAt(hitPoint.point);
                Debug.Log(hitPoint.transform.name);
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(cameraManager.cameraTransform.localEulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0); //Just faces the camera direction if raycast has no hit point, sky etc...
            }
        }
        else
        {
            //give ammo velocity
            if (cameraManager.currentLockOnTarget != null)
            {
                //SInce while locked we are always facing our target we can copy our facing direction to our arrows facing direction when fired
                Quaternion arrowRotation = Quaternion.LookRotation(cameraManager.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                liveArrow.transform.rotation = arrowRotation;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(cameraManager.cameraPivot.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0); //face the camera direction
            }
        }

        rigidbody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardVelocity * 3); //Adding forward force to the arrow itself
        rigidbody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upwardVelocity * 3); //Some rise
        rigidbody.useGravity = playerInventoryManager.currentAmmo.useGravity; //Incase we don't want it to fall over time
        rigidbody.mass = playerInventoryManager.currentAmmo.ammoMass; //Something to tweak
        liveArrow.transform.parent = null;
        //destroy previous loaded arrow fx
        // set live arrow damage
        damageCollider.characterManager = playerManager;
        damageCollider.ammoItem = playerInventoryManager.currentAmmo;
        damageCollider.physicalDamage = playerInventoryManager.currentAmmo.physicalDamage;
    }

    private void PerformRBRangedAction()
    {
        if (playerStatsManager.currentStamina <= 0) 
            return;

        playerAnimatorManager.EraseHandIKWeapon();
        playerAnimatorManager.animator.SetBool("isUsingRightHand", true);

        if (!playerManager.isHoldingArrow)
        {
            // If we have ammo
            if (!playerManager.isHoldingArrow)
            {
                if (playerInventoryManager.currentAmmo != null)
                {
                    DrawArrowAction();
                }
                else
                {
                    //Otherwise play an animation to indicate we are out of ammo
                    playerAnimatorManager.PlayTargetAnimation("shrug", true, true);
                }
            }
            
            //fire the arrow when we realease RB
            
        }
    }

    private void performLBBlockingAction()
    {
        if (playerManager.isInteracting)
            return;

        if (playerManager.isBlocking)
            return;

        playerAnimatorManager.PlayTargetAnimation("Block_Start", false, true, true);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;
    }

    private void PerformLBAimingAction()
    {
        if (playerManager.isAiming)
            return;

        inputManager.uiManager.crossHair.SetActive(true);
        playerManager.isAiming = true;
    }

    private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
    {
        if (playerManager.isInteracting)
            return;

        if (weapon.weaponType == WeaponType.FaithCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("shrug", true, true);
                }
            }
        }
        else if (weapon.weaponType == WeaponType.PyromancyCaster)
        {
            if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
            {
                if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                {
                    playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("shrug", true, true);
                }
            }
        }
    }

    private void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting)
            return;

        if (isTwoHanding)
        {
            // If we are two handing perform weapon art for right weapon
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation(weapon_art, true, true);
        }
    }

    private void SucessfullyCastSpell()
    {
        playerInventoryManager.currentSpell.SucessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraManager, playerWeaponSlotManager, playerManager.isUsingLeftHand);
        playerAnimatorManager.animator.SetBool("isFiringSpell", true);
    }
   
    public void AttemptBackStabOrRiposte()
    {
        if (playerStatsManager.currentStamina <= 0)
            return;

        RaycastHit hit;

        if (Physics.Raycast(inputManager.criticalRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null)
            {
                // Check for team I.D (So you cant back stab friends or yourself?)
                // Pull is into a transform behind the enemy so the backstab looks clean
                playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                // rotate us towards that transform
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;
                // play animation
                playerAnimatorManager.PlayTargetAnimation("Back Stab", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true, true);
                
                // make enemy play animation
                // do damage
            }
        }
        else if (Physics.Raycast(inputManager.criticalRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
        {
            // Check for team I.D (future task)
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerAnimatorManager.PlayTargetAnimation("Riposte", true, true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true, true);
            }
        }
    }
}
