using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeactivate : EventEffect
{
    [SerializeField] private GameObject[] targets;
    public override void Perform()
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(false);
        }
    }
}
