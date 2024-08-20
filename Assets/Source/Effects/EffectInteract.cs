using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectInteract : EventEffect
{
    [SerializeField] private Interaction interaction;

    public override void Perform()
    {
        interaction.Interact();
    }
}
