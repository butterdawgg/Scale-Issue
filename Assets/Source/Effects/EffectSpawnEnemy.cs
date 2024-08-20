using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawnEnemy : EventEffect
{
    [SerializeField] private Enemy prototype;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private int enemyID;

    public override void Perform()
    {
        if (SerializeManager.GetEnemyDefeatedStatus(enemyID))
            return;

        Enemy enemy = Instantiate(prototype.gameObject, spawnPoint,
            Quaternion.identity, default).GetComponent<Enemy>();

        enemy.ID = enemyID;
    }
}
