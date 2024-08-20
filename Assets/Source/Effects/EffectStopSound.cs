using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectStopSound : EventEffect
{
    [SerializeField] private string soundName;

    public override void Perform()
    {
        AudioManager.Instance.StopSound(soundName);
    }
}
