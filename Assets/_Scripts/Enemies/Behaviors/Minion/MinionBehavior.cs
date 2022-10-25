using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehavior : EnemyBehavior
{
    protected MinionEnemy minion;

    public void Setup(MinionEnemy thisEnemy)
    {
        this.minion = thisEnemy;
        this.enemy = thisEnemy;
    }
}
