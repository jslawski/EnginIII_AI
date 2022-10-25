using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemyBehavior : EnemyBehavior
{
    protected MinionEnemy minionEnemy;

    public void Setup(MinionEnemy thisEnemy)
    {
        this.minionEnemy = thisEnemy;
        this.enemy = thisEnemy;
    }
}
