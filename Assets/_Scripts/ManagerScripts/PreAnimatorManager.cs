using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreAnimatorManager : MonoBehaviour
{
    public Animator animator;
    public bool canRotate;

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool useRootMotion = false)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.SetBool("canRotate", false);
        animator.SetBool("isUsingRootMotion", useRootMotion);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public virtual void TakeCriticalDamageAnimationEvent()
    {

    }
}
