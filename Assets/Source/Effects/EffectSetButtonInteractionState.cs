using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSetButtonInteractionState : EventEffect
{
    [SerializeField] private ButtonController button;
    [SerializeField] private bool setStateTo = true;

    public override void Perform()
    {
        button.SetInteractions(setStateTo);
    }
}
