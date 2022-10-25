using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBehavior : EnemyBehavior
{
    protected BossEnemy bossEnemy;

    public void Setup(BossEnemy thisEnemy)
    {
        this.bossEnemy = thisEnemy;
        this.enemy = thisEnemy;
    }
}
