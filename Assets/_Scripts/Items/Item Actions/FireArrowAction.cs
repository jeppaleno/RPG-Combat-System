using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
public class FireArrowAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        //Create the Live arrow instantiation location
        ArrowInstantiationLocation arrowInstantiationLocation;
        arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

        //animate the bow firing the arrow
        Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("isDrawn", false);
        bowAnimator.Play("Bow_ONLY_Fire_01");
        Destroy(character.characterEffectsManager.currentRangeFX); //Destroys loaded arrow model

        //Reset the players holding arrow flag
        character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true, true);
        character.animator.SetBool("isHoldingArrow", false);

        //Create and fire the live arrow
        GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position, character.cameraManager.cameraPivot.rotation);
        Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

        if (character.isAiming)
        {
            Ray ray = character.cameraManager.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint, 100.0f))
            {
                liveArrow.transform.LookAt(hitPoint.point);
                Debug.Log(hitPoint.transform.name);
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(character.cameraManager.cameraTransform.localEulerAngles.x, character.lockOnTransform.eulerAngles.y, 0); //Just faces the camera direction if raycast has no hit point, sky etc...
            }
        }
        else
        {
            //give ammo velocity
            if (character.cameraManager.currentLockOnTarget != null)
            {
                //SInce while locked we are always facing our target we can copy our facing direction to our arrows facing direction when fired
                Quaternion arrowRotation = Quaternion.LookRotation(character.cameraManager.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                liveArrow.transform.rotation = arrowRotation;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(character.cameraManager.cameraPivot.eulerAngles.x, character.lockOnTransform.eulerAngles.y, 0); //face the camera direction
            }
        }

        rigidbody.AddForce(liveArrow.transform.forward * character.playerInventoryManager.currentAmmo.forwardVelocity * 3); //Adding forward force to the arrow itself
        rigidbody.AddForce(liveArrow.transform.up * character.playerInventoryManager.currentAmmo.upwardVelocity * 3); //Some rise
        rigidbody.useGravity = character.playerInventoryManager.currentAmmo.useGravity; //Incase we don't want it to fall over time
        rigidbody.mass = character.playerInventoryManager.currentAmmo.ammoMass; //Something to tweak
        liveArrow.transform.parent = null;
        //destroy previous loaded arrow fx
        // set live arrow damage
        damageCollider.characterManager = character;
        damageCollider.ammoItem = character.playerInventoryManager.currentAmmo;
        damageCollider.physicalDamage = character.playerInventoryManager.currentAmmo.physicalDamage;
    }
}
