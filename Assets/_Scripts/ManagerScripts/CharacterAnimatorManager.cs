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

    bool handIKWeightsReset = false;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        rigBuilder = GetComponent<RigBuilder>();
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
