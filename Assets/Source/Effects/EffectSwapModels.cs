using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSwapModels : EventEffect
{
    [SerializeField] private List<ModelSwapper> modelSwappers;

    public override void Perform()
    {
        foreach (var modelSwapper in modelSwappers)
        {
            modelSwapper.Interact();
        }
    }
}
