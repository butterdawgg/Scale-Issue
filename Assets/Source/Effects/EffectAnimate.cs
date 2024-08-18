using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimate : EventEffect
{
    [SerializeField] private GameObject activationTarget;
    [SerializeField] private string animationTrigger;

    private Animator animator;

    private void Awake()
    {
        animator = activationTarget.GetComponent<Animator>();
    }

    public override void Perform()
    {
        animator.SetTrigger(animationTrigger);
    }
}
