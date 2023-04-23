using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerInventoryManager playerInventory;

    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    //Chest and body armour etc

    public BlockingCollider blockingCollider;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerInventory = GetComponent<PlayerInventoryManager>(); //getComponent instead?
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
    }

    private void Start()
    {
        helmetModelChanger.UnEquipAllHelmetModels();
        helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
    }

    public void OpenBlockingCollider()
    {
        if (inputManager.twohandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
        }
        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
