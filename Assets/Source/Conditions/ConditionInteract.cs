using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionInteract : EventCondition
{
    [SerializeField] private InteractionConditionSetter[] interactions;

    private bool isMet = false;

    private void Update()
    {
        isMet = true;

        foreach (var interaction in interactions)
        {
            if (interaction.Interacted)
            {
                isMet = false;
            }
        }
    }

    public override void Set()
    {
        isMet = false;
    }

    public override bool Check()
    {
        return isMet;
    }
}
