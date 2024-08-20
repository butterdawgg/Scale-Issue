using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimate : EventEffect
{
    [SerializeField] private Animator target;
    [SerializeField] private string trigger;

    public override void Perform()
    {
        target.SetTrigger(trigger);
    }
}
