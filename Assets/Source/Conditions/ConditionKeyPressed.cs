using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionKeyPressed : EventCondition
{
    [SerializeField] private KeyCode key;

    private bool activated = false;

    void Update()
    {
        if (activated)
            return;

        if (Input.GetKeyDown(key))
            activated = true;
    }

    public override void Set()
    {
        activated = false;
    }

    public override bool Check()
    {
        return activated;
    }
}
