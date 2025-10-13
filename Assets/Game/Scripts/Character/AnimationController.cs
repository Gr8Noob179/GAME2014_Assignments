using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetValue(EAnimationParameter animation, int value)
    {
        animator.SetInteger(Enum.GetName(typeof(EAnimationParameter), animation), value);
    }

    public void SetValue(EAnimationParameter animation, float value)
    {
        animator.SetFloat(Enum.GetName(typeof(EAnimationParameter), animation), value);
    }

    public void SetValue(EAnimationParameter animation, bool value)
    {
        animator.SetBool(Enum.GetName(typeof(EAnimationParameter), animation), value);
    }

    public void SetValue(EAnimationParameter animation)
    {
        animator.SetTrigger(Enum.GetName(typeof(EAnimationParameter), animation));
    }

    public void SetAnimator(RuntimeAnimatorController animatorController)
    {
        animator.runtimeAnimatorController = animatorController;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public static AnimationController Get(GameObject owner)
    {
        return owner.GetComponentInChildren<AnimationController>();
    }
}

public enum EAnimationParameter
{
    DirectionX,
    Attack,
    Jump,
    Hit,
    Death,
}