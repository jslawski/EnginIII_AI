using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : EnemyBehavior
{
    protected BossEnemy boss;

    public void Setup(BossEnemy thisEnemy)
    {
        this.boss = thisEnemy;
        this.enemy = thisEnemy;        
    }
}
