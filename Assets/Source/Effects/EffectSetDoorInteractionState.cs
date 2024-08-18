using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSetDoorInteractionState : EventEffect
{
    [SerializeField] private Door door;
    [SerializeField] private bool setStateTo = true;

    public override void Perform()
    {
        door.SetInteractions(setStateTo);
    }
}
