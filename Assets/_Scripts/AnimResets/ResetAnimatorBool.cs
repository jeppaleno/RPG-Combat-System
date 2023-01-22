using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string targetBool;
    public bool status;

    public string isUsingRootMotionBool;
    public bool isUsingRootMotionStatus;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, status);
        animator.SetBool(isUsingRootMotionBool, isUsingRootMotionStatus);
    }
}
