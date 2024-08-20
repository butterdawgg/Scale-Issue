using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSetInteractableLockedState : EventEffect
{
    [SerializeField] private Interactable[] interactables;
    [SerializeField] private bool lockedState;

    public override void Perform()
    {
        foreach (var interactable in interactables)
        {
            interactable.SetLockedState(lockedState);
        }
    }
}
