using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
public class FireArrowAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        PlayerManager player = character as PlayerManager;
        //Create the Live arrow instantiation location
        ArrowInstantiationLocation arrowInstantiationLocation;
        arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

        //animate the bow firing the arrow
        Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("isDrawn", false);
        bowAnimator.Play("Bow_ONLY_Fire_01");
        Destroy(character.characterEffectsManager.instantiatedFXModel); //Destroys loaded arrow model

        //Reset the players holding arrow flag
        character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true, true);
        character.animator.SetBool("isHoldingArrow", false);

        //FIRE THE ARROW AS A PLAYER CHARACTER
        if (player != null)
        {
            //Create and fire the live arrow
            GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, player.cameraManager.cameraPivot.rotation);
            Rigidbody rigidbody = liveArrow.GetComponent<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

            if (character.isAiming)
            {
                Ray ray = player.cameraManager.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitPoint;

                if (Physics.Raycast(ray, out hitPoint, 100.0f))
                {
                    liveArrow.transform.LookAt(hitPoint.point);
                    Debug.Log(hitPoint.transform.name);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraManager.cameraTransform.localEulerAngles.x, character.lockOnTransform.eulerAngles.y, 0); //Just faces the camera direction if raycast has no hit point, sky etc...
                }
            }
            else
            {
                //give ammo velocity
                if (player.cameraManager.currentLockOnTarget != null)
                {
                    //SInce while locked we are always facing our target we can copy our facing direction to our arrows facing direction when fired
                    Quaternion arrowRotation = Quaternion.LookRotation(player.cameraManager.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraManager.cameraPivot.eulerAngles.x, character.lockOnTransform.eulerAngles.y, 0); //face the camera direction
                }
            }

            rigidbody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardVelocity * 3); //Adding forward force to the arrow itself
            rigidbody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upwardVelocity * 3); //Some rise
            rigidbody.useGravity = player.playerInventoryManager.currentAmmo.useGravity; //Incase we don't want it to fall over time
            rigidbody.mass = player.playerInventoryManager.currentAmmo.ammoMass; //Something to tweak
            liveArrow.transform.parent = null;
            //destroy previous loaded arrow fx
            // set live arrow damage
            damageCollider.characterManager = character;
            damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
            damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
        }
        //---------------------- FIRE THE ARROW AS AN A.I CHARACTER -----------------------
        else
        {
            EnemyManager enemy = character as EnemyManager;

            //Create and fire the live arrow
            GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, Quaternion.identity);
            Rigidbody rigidbody = liveArrow.GetComponent<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();


            //give ammo velocity
            if (enemy.currentTarget != null)
            {
                //SInce while locked we are always facing our target we can copy our facing direction to our arrows facing direction when fired
                Quaternion arrowRotation = Quaternion.LookRotation(enemy.currentTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                liveArrow.transform.rotation = arrowRotation;
            }

            rigidbody.AddForce(liveArrow.transform.forward * enemy.characterInventoryManager.currentAmmo.forwardVelocity * 3); //Adding forward force to the arrow itself
            rigidbody.AddForce(liveArrow.transform.up * enemy.characterInventoryManager.currentAmmo.upwardVelocity * 3); //Some rise
            rigidbody.useGravity = enemy.characterInventoryManager.currentAmmo.useGravity; //Incase we don't want it to fall over time
            rigidbody.mass = enemy.characterInventoryManager.currentAmmo.ammoMass; //Something to tweak
            liveArrow.transform.parent = null;

            // set live arrow damage
            damageCollider.characterManager = character;
            damageCollider.ammoItem = enemy.characterInventoryManager.currentAmmo;
            damageCollider.physicalDamage = enemy.characterInventoryManager.currentAmmo.physicalDamage;
            damageCollider.teamIDNumber = enemy.characterStatsManager.teamIDNumber;
        }
    }
        
        
}
