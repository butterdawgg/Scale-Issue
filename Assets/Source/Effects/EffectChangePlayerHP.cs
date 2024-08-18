using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectChangePlayerHP : EventEffect
{
    [SerializeField] private float deltaHealth;

    public override void Perform()
    {
        Player.Instance.Health += deltaHealth;
    }
}
