using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBoolAI : ResetAnimatorBool
{
    public string isPhaseShifting = "isPhaseShifting";
    public bool isPhaseShiftingStatus = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(isPhaseShifting, isPhaseShiftingStatus);
    }
}
