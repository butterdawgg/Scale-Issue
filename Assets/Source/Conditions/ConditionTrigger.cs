using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionTrigger : EventCondition
{
    [SerializeField] private Transform triggerPoint;
    [SerializeField] private float triggerRadius;

    private bool activated = false;

    private void Update()
    {
        if (activated)
            return;

        float distance = (Player.Instance.Position - triggerPoint.position).magnitude;

        if (distance <= triggerRadius)
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
