using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimatorManager : MonoBehaviour
{
    public Animator animator;
    protected CharacterManager characterManager;
    protected CharacterStatsManager characterStatsManager;

    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    bool handIKWeightsReset = false;

    protected virtual void Awake()
    {
        characterManager = GetComponent<CharacterManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        rigBuilder = GetComponent<RigBuilder>();
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false, bool canRotate = false, bool mirrorAnim = false)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.SetBool("canRotate", canRotate);
        animator.SetBool("isUsingRootMotion", useRootMotion);
        animator.SetBool("isMirrored", mirrorAnim);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayTargetAnimationWithRootRotation(string targetAnimation, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool("isRotatingWithRootMotion", true);
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public virtual void CanRotate()
    {
        animator.SetBool("canRotate", true);
    }

    public virtual void stopRotation()
    {
        animator.SetBool("canRotate", false);
    }

    public virtual void EnableCombo()
    {
        animator.SetBool("canDoCombo", true);
    }

    public virtual void DisableCombo()
    {
        animator.SetBool("canDoCombo", false);
    }

    public virtual void EnableIsParrying()
    {
        characterManager.isParrying = true;
    }

    public virtual void DisableIsParrying()
    {
        characterManager.isParrying = false;
    }

    public virtual void EnableCanBeRiposted()
    {
        characterManager.canBeRiposted = true;
    }

    public virtual void DisableCanBeRiposted()
    {
        characterManager.canBeRiposted = false;
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {
        characterStatsManager.TakeDamageNoAnimation(characterManager.pendingCriticalDamage, 0);
        characterManager.pendingCriticalDamage = 0;
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
        if (characterManager.isInteracting)
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
