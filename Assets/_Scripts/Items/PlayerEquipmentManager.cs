using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    PlayerManager player;

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
        player = GetComponent<PlayerManager>();

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

    public void EquipAllEquipmentModelsOnStart()
    {
        //HELMET EQUIPMENT
        helmetModelChanger.UnEquipAllHelmetModels();

        if (player.playerInventoryManager.currentHelmetEquipment != null)
        {
            nakedHeadModel.SetActive(false);
            helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
            player.playerStatsManager.physicalDamageAbsoptionHead = player.playerInventoryManager.currentHandEquipment.physicalDefense;
            //Debug.Log("Head Absorption is " + player.playerStatsManager.physicalDamageAbsoptionHead + "%");
        }
        else
        {
            nakedHeadModel.SetActive(true);
            player.playerStatsManager.physicalDamageAbsoptionHead = 0;
        }
        
        //TORSO EQUIPMENT
        torsoModelChanger.UnEquipAllTorsoModels();
        upperLeftArmModelChanger.UnEquipAllModels();
        upperRightArmModelChanger.UnEquipAllModels();

        if (player.playerInventoryManager.currentBodyEquipment != null)
        {
            torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentBodyEquipment.torsoModelName);
            upperLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperLeftArmModelName);
            upperRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentBodyEquipment.upperRightArmModelName);
            player.playerStatsManager.physicalDamageAbsoptionBody = player.playerInventoryManager.currentBodyEquipment.physicalDefense;
            //Debug.Log("Body Absorption is " + player.playerStatsManager.physicalDamageAbsoptionBody + "%");
        }
        else
        {
            torsoModelChanger.EquipTorsoModelByName(nakedTorsoModel);
            upperLeftArmModelChanger.EquipModelByName(nakedUpperLeftArm);
            upperRightArmModelChanger.EquipModelByName(nakedUpperRightArm);
            player.playerStatsManager.physicalDamageAbsoptionBody = 0;
        }

        //LEG EQUIPMENT
        hipModelChanger.UnEquipAllHipModels();
        leftLegModelChanger.UnEquipAllLegModels();
        rightLegModelChanger.UnEquipAllLegModels();

        if (player.playerInventoryManager.currentLegEquipment != null)
        {
            hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
            leftLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegName);
            rightLegModelChanger.EquipLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegName);
            player.playerStatsManager.physicalDamageAbsoptionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
            //Debug.Log("Leg Absorption is " + player.playerStatsManager.physicalDamageAbsoptionLegs + "%");
        }
        else
        {
            hipModelChanger.EquipHipModelByName(nakedHipModel);
            leftLegModelChanger.EquipLegModelByName(nakedLeftLeg);
            rightLegModelChanger.EquipLegModelByName(nakedRightLeg);
            player.playerStatsManager.physicalDamageAbsoptionLegs = 0;
        }

        //HAND EQUIPMENT
        lowerLeftArmModelChanger.UnEquipAllModels();
        lowerRightArmModelChanger.UnEquipAllModels();
        leftHandModelChanger.UnEquipAllModels();
        rightHandModelChanger.UnEquipAllModels();

        if (player.playerInventoryManager.currentHandEquipment != null)
        {
            lowerLeftArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
            lowerRightArmModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
            leftHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
            rightHandModelChanger.EquipModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);
            player.playerStatsManager.physicalDamageAbsoptionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
            //Debug.Log("Hand Absorption is " + player.playerStatsManager.physicalDamageAbsoptionHands + "%");
        }
        else
        {
            lowerLeftArmModelChanger.EquipModelByName(nakedLowerLeftArm);
            lowerRightArmModelChanger.EquipModelByName(nakedLowerRightArm);
            leftHandModelChanger.EquipModelByName(nakedLeftHand);
            rightHandModelChanger.EquipModelByName(nakedRightHand);
            player.playerStatsManager.physicalDamageAbsoptionHands = 0;
        }
    }

    public void OpenBlockingCollider()
    {
        if (player.inputManager.twohandFlag)
        {
            blockingCollider.SetColliderDamageAbsorption(player.playerInventoryManager.rightWeapon);
        }
        else
        {
            blockingCollider.SetColliderDamageAbsorption(player.playerInventoryManager.leftWeapon);
        }
        blockingCollider.EnableBlockingCollider();
    }

    public void CloseBlockingCollider()
    {
        blockingCollider.DisableBlockingCollider();
    }
}
