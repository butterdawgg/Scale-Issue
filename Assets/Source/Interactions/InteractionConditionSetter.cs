using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionConditionSetter : Interaction
{
    public bool Interacted { get; set; } = false;

    public override void Interact()
    {
        Interacted = true;
    }
}
