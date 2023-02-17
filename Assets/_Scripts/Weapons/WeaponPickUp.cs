using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager); //Pick up the item and add it to the player inventory
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        Character character;
        AnimatorManager animatorManager;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        character = playerManager.GetComponent<Character>();
        animatorManager = playerManager.GetComponentInChildren<AnimatorManager>();

        character.playerRigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
        animatorManager.PlayTargetAnimation("Pick Up Item", true); //Plays the animation of looting the item
        playerInventory.weaponInventory.Add(weapon);
        playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture; // Changing the item icon to the icon of the object where it's stored
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
