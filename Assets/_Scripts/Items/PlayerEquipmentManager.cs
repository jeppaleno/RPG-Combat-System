using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerInventoryManager playerInventory;

    [Header("Equipment Model Changers")]
    HelmetModelChanger helmetModelChanger;
    TorsoModelChanger torsoModelChanger;
    //Chest and body armour etc

    [Header("Default Naked Models")]
    public GameObject nakedHeadModel;
    public string nakedTorsoModel;
    //NakedLegModelETC

    public BlockingCollider blockingCollider;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerInventory = GetComponent<PlayerInventoryManager>();
        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();

    }

    private void EquipAllEquipmentModelsOnStart()
    {
        helmetModelChanger.UnEquipAllHelmetModels();

        if (playerInventory.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
        }
        else
        {
            nakedHeadModel.SetActive(true);
        }
        
        torsoModelChanger.UnEquipAllTorsoModels();

        if (playerInventory.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
        }
        else
        {
            torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
        }
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
