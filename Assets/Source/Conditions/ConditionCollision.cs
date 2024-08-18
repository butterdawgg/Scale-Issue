using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionCollision : EventCondition
{
    [SerializeField] private Transform collisionTarget;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != collisionTarget || activated)
            return;

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
