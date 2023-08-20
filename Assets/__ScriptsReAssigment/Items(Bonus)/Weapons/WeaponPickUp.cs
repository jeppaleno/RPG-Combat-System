using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    // This is a unique ID for this item spawn in the game world, each item we place in your world should have it's own unique ID
    [Header("Item Information")]
    [SerializeField] int itemPickUpID;
    [SerializeField] bool hasBeenLooted;

    [Header("item")]
    public WeaponItem weapon;


    protected override void Start()
    {
        base.Start();

        // If the save data does not contain this item, we must have never looted it, so we can add it to the list and list it as not looted
        if (!WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
        {
            WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, false);
        }

        hasBeenLooted = WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld[itemPickUpID];

        if (hasBeenLooted)
        {
            gameObject.SetActive(false);
        }
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        // Notify the character data this item has been looted from the world, so it does not spawn again
        if (WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.ContainsKey(itemPickUpID))
        {
            WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Remove(itemPickUpID);
        }

        // Saves the pick up to our save data so it does not spawn again when we realod the are
        WorldSaveGameManager.instance.currentCharacterSaveData.itemsInWorld.Add(itemPickUpID, true);

        hasBeenLooted = true;

        //Pick up the item and add it to the player inventory
        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventoryManager playerInventory;
        PlayerLocomotionManager character;
        PlayerAnimatorManager animatorManager;

        playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
        character = playerManager.GetComponent<PlayerLocomotionManager>();
        animatorManager = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

        //character.playerRigidbody.velocity = Vector3.zero; //Stops the player from moving whilst picking up item
        animatorManager.PlayTargetAnimation("Pick Up Item", true); //Plays the animation of looting the item
        playerInventory.weaponInventory.Add(weapon);
        playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture; // Changing the item icon to the icon of the object where it's stored
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
