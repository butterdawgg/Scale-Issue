using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDefeatEnemies : EventCondition
{
    private Enemy[] targets = new Enemy[0];

    private bool isMet = false;

    void Update()
    {
        if (targets.Length <= 0)
            return;

        isMet = true;

        foreach (Enemy target in targets)
        {
            if (target.Health > 0)
            {
                isMet = false;

                return;
            }
        }
    }

    public override void Set()
    {
        isMet = false;

        targets = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    }

    public override bool Check()
    {
        return isMet;
    }
}
