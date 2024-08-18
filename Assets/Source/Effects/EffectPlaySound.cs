using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlaySound : EventEffect
{
    [SerializeField] private string soundName;

    public override void Perform()
    {
        AudioManager.Instance.PlaySound(soundName);
    }
}
