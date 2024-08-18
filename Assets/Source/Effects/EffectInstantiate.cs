using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectInstantiate : EventEffect
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    public override void Perform()
    {
        Instantiate(prefab, position, Quaternion.Euler(rotation));
    }
}
