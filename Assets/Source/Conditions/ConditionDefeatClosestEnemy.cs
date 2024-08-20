using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionDefeatClosestEnemy : EventCondition
{
    private Enemy target;

    private bool isMet = false;

    private void Update()
    {
        if (target == null)
            return;

        if (target.Health > 0)
            return;

        isMet = true;
    }

    public override void Set()
    {
        isMet = false;

        Enemy[] targets = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        if (targets.Length <= 0)
            return;

        target = targets[0];

        foreach (Enemy enemy in targets)
        {
            float enemyDistance = (enemy.transform.position - Player.Instance.Position).magnitude;
            float targetDistance = (target.transform.position - Player.Instance.Position).magnitude;

            if (enemyDistance < targetDistance)
            {
                target = enemy;
            }
        }
    }

    public override bool Check()
    {
        return isMet;
    }
}
