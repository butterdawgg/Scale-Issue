using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectShakeCamera : EventEffect
{
    [SerializeField] private float duration;
    [SerializeField] private float amplitude;

    public override void Perform()
    {
        Player.Instance.ShakeCamera(amplitude, duration);
    }
}
