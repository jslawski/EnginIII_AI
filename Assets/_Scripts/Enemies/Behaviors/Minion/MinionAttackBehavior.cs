using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttackBehavior : MinionBehavior
{
    public override void ExecuteBehavior()
    {
        this.enemy.TurnTowardsPlayer();

        if (this.enemy.PlayerIsInSight())
        {
            if (this.enemy.enemyAttackCoroutine == null)
            {
                this.enemy.attackCooldown = this.enemy.secondsBetweenAttacks;
                this.enemy.Attack();
            }
        }
    }
}
