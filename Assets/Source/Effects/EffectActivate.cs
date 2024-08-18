using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActivate : EventEffect
{
    [SerializeField] private GameObject activationTarget;

    private void Awake()
    {
        activationTarget.SetActive(false);
    }

    public override void Perform()
    {
        activationTarget.SetActive(true);
    }
}
