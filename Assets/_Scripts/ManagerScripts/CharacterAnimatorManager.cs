using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimatorManager : MonoBehaviour
{
    protected CharacterManager character;

    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    [Header("DAMAGE ANIMATIONS")]
    //LIGHT
    [HideInInspector] public string Damage_Forward_Light_01 = "Damage_Forward_Light_01";
    [HideInInspector] public string Damage_Forward_Light_02 = "Damage_Forward_Light_02";

    [HideInInspector] public string Damage_Back_Light_01 = "Damage_Back_Light_01";
    [HideInInspector] public string Damage_Back_Light_02 = "Damage_Back_Light_02";

    [HideInInspector] public string Damage_Left_Light_01 = "Damage_Left_Light_01";
    [HideInInspector] public string Damage_Left_Light_02 = "Damage_Left_Light_02";

    [HideInInspector] public string Damage_Right_Light_01 = "Damage_Right_Light_01";
    [HideInInspector] public string Damage_Right_Light_02 = "Damage_Right_Light_02";

    //MEDIUM
    [HideInInspector] public string Damage_Forward_Medium_01 = "Damage_Forward_Medium_01";
    [HideInInspector] public string Damage_Forward_Medium_02 = "Damage_Forward_Medium_02";

    [HideInInspector] public string Damage_Back_Medium_01 = "Damage_Back_Medium_01";
    [HideInInspector] public string Damage_Back_Medium_02 = "Damage_Back_Medium_02";

    [HideInInspector] public string Damage_Left_Medium_01 = "Damage_Left_Medium_01";
    [HideInInspector] public string Damage_Left_Medium_02 = "Damage_Left_Medium_02";

    [HideInInspector] public string Damage_Right_Medium_01 = "Damage_Right_Medium_01";
    [HideInInspector] public string Damage_Right_Medium_02 = "Damage_Right_Medium_02";

    //HEAVY
    [HideInInspector] public string Damage_Forward_Heavy_01 = "Damage_Forward_Heavy_01";
    [HideInInspector] public string Damage_Forward_Heavy_02 = "Damage_Forward_Heavy_02";

    [HideInInspector] public string Damage_Back_Heavy_01 = "Damage_Back_Heavy_01";
    [HideInInspector] public string Damage_Back_Heavy_02 = "Damage_Back_Heavy_02";

    [HideInInspector] public string Damage_Left_Heavy_01 = "Damage_Left_Heavy_01";
    [HideInInspector] public string Damage_Left_Heavy_02 = "Damage_Left_Heavy_02";

    [HideInInspector] public string Damage_Right_Heavy_01 = "Damage_Right_Heavy_01";
    [HideInInspector] public string Damage_Right_Heavy_02 = "Damage_Right_Heavy_02";

    //COLOSSAL
    [HideInInspector] public string Damage_Forward_Colossal_01 = "Damage_Forward_Colossal_01";
    [HideInInspector] public string Damage_Forward_Colossal_02 = "Damage_Forward_Colossal_02";

    [HideInInspector] public string Damage_Back_Colossal_01 = "Damage_Back_Colossal_01";
    [HideInInspector] public string Damage_Back_Colossal_02 = "Damage_Back_Colossal_02";

    [HideInInspector] public string Damage_Left_Colossal_01 = "Damage_Left_Colossal_01";
    [HideInInspector] public string Damage_Left_Colossal_02 = "Damage_Left_Colossal_02";

    [HideInInspector] public string Damage_Right_Colossal_01 = "Damage_Right_Colossal_01";
    [HideInInspector] public string Damage_Right_Colossal_02 = "Damage_Right_Colossal_02";

    [HideInInspector] public List<string> Damage_Animations_Light_Forward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Light_Backward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Light_Right = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Light_Left = new List<string>();

    [HideInInspector] public List<string> Damage_Animations_Medium_Forward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Medium_Backward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Medium_Right = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Medium_Left = new List<string>();

    [HideInInspector] public List<string> Damage_Animations_Heavy_Forward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Heavy_Backward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Heavy_Right = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Heavy_Left = new List<string>();

    [HideInInspector] public List<string> Damage_Animations_Colossal_Forward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Colossal_Backward = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Colossal_Right = new List<string>();
    [HideInInspector] public List<string> Damage_Animations_Colossal_Left = new List<string>();



    bool handIKWeightsReset = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }

    protected virtual void Start()
    {
        Damage_Animations_Light_Forward.Add(Damage_Forward_Light_01);
        Damage_Animations_Light_Forward.Add(Damage_Forward_Light_02);

        Damage_Animations_Light_Backward.Add(Damage_Back_Light_01);
        Damage_Animations_Light_Backward.Add(Damage_Back_Light_02);

        Damage_Animations_Light_Left.Add(Damage_Left_Light_01);
        Damage_Animations_Light_Left.Add(Damage_Left_Light_02);

        Damage_Animations_Light_Right.Add(Damage_Right_Light_01);
        Damage_Animations_Light_Right.Add(Damage_Right_Light_02);


        Damage_Animations_Medium_Forward.Add(Damage_Forward_Medium_01);
        Damage_Animations_Medium_Forward.Add(Damage_Forward_Medium_02);

        Damage_Animations_Medium_Backward.Add(Damage_Back_Medium_01);
        Damage_Animations_Medium_Backward.Add(Damage_Back_Medium_02);

        Damage_Animations_Medium_Left.Add(Damage_Left_Medium_01);
        Damage_Animations_Medium_Left.Add(Damage_Left_Medium_02);

        Damage_Animations_Medium_Right.Add(Damage_Right_Medium_01);
        Damage_Animations_Medium_Right.Add(Damage_Right_Medium_02);


        Damage_Animations_Heavy_Forward.Add(Damage_Forward_Heavy_01);
        Damage_Animations_Heavy_Forward.Add(Damage_Forward_Heavy_02);

        Damage_Animations_Heavy_Backward.Add(Damage_Back_Heavy_01);
        Damage_Animations_Heavy_Backward.Add(Damage_Back_Heavy_02);

        Damage_Animations_Heavy_Left.Add(Damage_Left_Heavy_01);
        Damage_Animations_Heavy_Left.Add(Damage_Left_Heavy_02);

        Damage_Animations_Heavy_Right.Add(Damage_Right_Heavy_01);
        Damage_Animations_Heavy_Right.Add(Damage_Right_Heavy_02);


        Damage_Animations_Colossal_Forward.Add(Damage_Forward_Colossal_01);
        Damage_Animations_Colossal_Forward.Add(Damage_Forward_Colossal_02);

        Damage_Animations_Colossal_Backward.Add(Damage_Back_Colossal_01);
        Damage_Animations_Colossal_Backward.Add(Damage_Back_Colossal_02);

        Damage_Animations_Colossal_Left.Add(Damage_Left_Colossal_01);
        Damage_Animations_Colossal_Left.Add(Damage_Left_Colossal_02);

        Damage_Animations_Colossal_Right.Add(Damage_Right_Colossal_01);
        Damage_Animations_Colossal_Right.Add(Damage_Right_Colossal_02);
    }

    public virtual void OnAnimatorMove()
    {
        if (character.isInteracting == false)
            return;

        Vector3 velocity = character.animator.deltaPosition;
        character.characterController.Move(velocity);
        character.transform.rotation *= character.animator.deltaRotation;
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false, bool canRotate = false, bool mirrorAnim = false)
    {
        character.animator.SetBool("isInteracting", isInteracting);
        character.animator.SetBool("canRotate", canRotate);
        character.animator.SetBool("isUsingRootMotion", useRootMotion);
        character.animator.SetBool("isMirrored", mirrorAnim);
        character.animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnimation, bool isInteracting)
    {
        character.animator.applyRootMotion = isInteracting;
        character.animator.SetBool("isRotatingWithRootMotion", true);
        character.animator.SetBool("isInteracting", isInteracting);
        character.animator.CrossFade(targetAnimation, 0.2f);
    }

    public string GetRandomDamageAnimationFromList(List<string> animationList)
    {
        int randomValue = Random.Range(0, animationList.Count);

        return animationList[randomValue];
    }

    public virtual void CanRotate()
    {
        character.animator.SetBool("canRotate", true);
    }

    public virtual void stopRotation()
    {
        character.animator.SetBool("canRotate", false);
    }

    public virtual void EnableCombo()
    {
        character.animator.SetBool("canDoCombo", true);
    }

    public virtual void DisableCombo()
    {
        character.animator.SetBool("canDoCombo", false);
    }

    public virtual void EnableIsParrying()
    {
        character.isParrying = true;
    }

    public virtual void DisableIsParrying()
    {
        character.isParrying = false;
    }

    public virtual void EnableCanBeRiposted()
    {
        character.canBeRiposted = true;
    }

    public virtual void DisableCanBeRiposted()
    {
        character.canBeRiposted = false;
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
        character.characterStatsManager.TakeDamageNoAnimation(character.pendingCriticalDamage, 0);
        character.pendingCriticalDamage = 0;
    }

    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
    {
        //Check if we are two handing our weapon
        if (isTwoHandingWeapon)
        {
            if (rightHandTarget != null)
            {
                //If we are, apply hand IK if needed
                rightHandConstraint.data.target = rightHandTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1; //Assign this from a weapon variable as it fits
                rightHandConstraint.data.targetRotationWeight = 1;
            }
            if (leftHandTarget != null)
            {
                leftHandConstraint.data.target = leftHandTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
            
        }
        else
        {
            //If not disable hand IK for now
            rightHandConstraint.data.target = null;
            leftHandConstraint.data.target = null;
        }
        rigBuilder.Build();
    }

    public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
    {
        if (character.isInteracting)
            return;

        if (handIKWeightsReset)
        {
            handIKWeightsReset = false;

            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.target = rightHandIK.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.target = leftHandIK.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
    }

    public virtual void EraseHandIKWeapon()
    {
        handIKWeightsReset = true;

        if (rightHandConstraint.data.target != null)
        {
            rightHandConstraint.data.targetPositionWeight = 0;
            rightHandConstraint.data.targetRotationWeight = 0;
        }

        if (leftHandConstraint.data.target != null)
        {
            leftHandConstraint.data.targetPositionWeight = 0;
            leftHandConstraint.data.targetRotationWeight = 0;
        }
    }

  
}
