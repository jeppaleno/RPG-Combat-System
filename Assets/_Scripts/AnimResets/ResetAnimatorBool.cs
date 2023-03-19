using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus = false;

    public string isFiringSpellBool = "isFiringSpell";
    public bool isFiringSpellStatus = false;
    //public string isUsingRootMotionBool;
    //public bool isUsingRootMotionStatus;

    public string isRotatingWithRootMotion = "isRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus = false;

    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(isRotatingWithRootMotion, isRotatingWithRootMotionStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        //animator.SetBool(isUsingRootMotionBool, isUsingRootMotionStatus);
    }
}
