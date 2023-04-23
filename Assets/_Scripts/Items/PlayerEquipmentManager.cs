using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerInventoryManager playerInventory;
    PlayerStatsManager playerStatsManager;

    [Header("Equipment Model Changers")]
    //Head Equipment 
    HelmetModelChanger helmetModelChanger;
    [Space]//Torso Equipment
    TorsoModelChanger torsoModelChanger;
    UpperLeftArmModelChanger upperLeftArmModelChanger;
    UpperRightArmModelChanger upperRightArmModelChanger;
    [Space]//Leg Equipment
    HipModelChanger hipModelChanger;
    LeftLegModelChanger leftLegModelChanger;
    RightLegModelChanger rightLegModelChanger;
    [Space] //Hand Equipment
    LowerLeftArmModelChanger lowerLeftArmModelChanger;
    LowerRightArmModelChanger lowerRightArmModelChanger;
    RightHandModelChanger rightHandModelChanger;
    LeftHandModelChanger leftHandModelChanger;


    [Header("Default Naked Models")]
    public GameObject nakedHeadModel;
    public string nakedTorsoModel;
    public string nakedUpperLeftArm;
    public string nakedUpperRightArm;
    public string nakedLowerLeftArm;
    public string nakedLowerRightArm;
    public string nakedLeftHand;
    public string nakedRightHand;
    public string nakedHipModel;
    public string nakedLeftLeg;
    public string nakedRightLeg;

    public BlockingCollider blockingCollider;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerInventory = GetComponent<PlayerInventoryManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();

        helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
        torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
        hipModelChanger = GetComponentInChildren<HipModelChanger>();
        leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
        rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
        upperLeftArmModelChanger = GetComponentInChildren<UpperLeftArmModelChanger>();
        upperRightArmModelChanger = GetComponentInChildren<UpperRightArmModelChanger>();
        lowerLeftArmModelChanger = GetComponentInChildren<LowerLeftArmModelChanger>();
        lowerRightArmModelChanger = GetComponentInChildren<LowerRightArmModelChanger>();
        leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
        rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
    }

    private void Start()
    {
        EquipAllEquipmentModelsOnStart();

    }

    private void EquipAllEquipmentModelsOnStart()
    {
        //HELMET EQUIPMENT
        helmetModelChanger.UnEquipAllHelmetModels();

        if (playerInventory.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            playerStatsManager.physicalDamageAbsoptionHead = playerInventory.currentHandEquipment.physicalDefense;
            Debug.Log("Head Absorption is " + playerStatsManager.physicalDamageAbsoptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            playerStatsManager.physicalDamageAbsoptionHead = 0;
        }
        
        //TORSO EQUIPMENT
        torsoModelChanger.UnEquipAllTorsoModels();
        upperLeftArmModelChanger.UnEquipAllModels();
        upperRightArmModelChanger.UnEquipAllModels();

        if (playerInventory.currentTorsoEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
            upperLeftArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(playerInventory.currentTorsoEquipment.upperRightArmModelName);
            playerStatsManager.physicalDamageAbsoptionBody = playerInventory.currentTorsoEquipment.physicalDefense;
            Debug.Log("Body Absorption is " + playerStatsManager.physicalDamageAbsoptionBody + "%");
        }
        else
        {
            torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
            upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
            upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
            playerStatsManager.physicalDamageAbsoptionBody = 0;
        }

        //LEG EQUIPMENT
        hipModelChanger.UnEquipAllHipModels();
        leftLegModelChanger.UnEquipAllLegModels();
        rightLegModelChanger.UnEquipAllLegModels();

        if (playerInventory.currentLegEquipment != null)
        {
            hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
            leftLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.leftLegName);
            rightLegModelChanger.EquipLegModelByName(playerInventory.currentLegEquipment.rightLegName);
            playerStatsManager.physicalDamageAbsoptionLegs = playerInventory.currentLegEquipment.physicalDefense;
            Debug.Log("Leg Absorption is " + playerStatsManager.physicalDamageAbsoptionLegs + "%");
        }
        else
        {
            hipModelChanger.EquipHipModelByName(nakedHipModel);
            leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
            rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
            playerStatsManager.physicalDamageAbsoptionLegs = 0;
        }

        //HAND EQUIPMENT
        lowerLeftArmModelChanger.UnEquipAllModels();
        lowerRightArmModelChanger.UnEquipAllModels();
        leftHandModelChanger.UnEquipAllModels();
        rightHandModelChanger.UnEquipAllModels();

        if (playerInventory.currentHandEquipment != null)
        {
            lowerLeftArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
            lowerRightArmModelChanger.EquipModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
            leftHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.leftHandModelName);
            rightHandModelChanger.EquipModelByName(playerInventory.currentHandEquipment.rightHandModelName);
            playerStatsManager.physicalDamageAbsoptionHands = playerInventory.currentHandEquipment.physicalDefense;
            Debug.Log("Hand Absorption is " + playerStatsManager.physicalDamageAbsoptionHands + "%");
        }
        else
        {
            lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
            lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
            leftHandModelChanger.EquipModelByName(nakedLeftHand);
            rightHandModelChanger.EquipModelByName(nakedRightHand);
            playerStatsManager.physicalDamageAbsoptionHands = 0;
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
