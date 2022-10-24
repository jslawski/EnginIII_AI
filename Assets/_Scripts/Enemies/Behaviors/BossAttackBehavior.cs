using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBehavior : EnemyBehavior
{
    private float attackAngle = 30.0f;

    private float secondsBetweenAttacks = 2.0f;

    private void TurnTowardsPlayer()
    {
        Vector3 targetDirection = (this.enemy.playerTarget.creatureRb.position - this.enemy.creatureRb.position).normalized;
        float targetZRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        this.enemy.creatureRb.rotation = Quaternion.RotateTowards(this.enemy.creatureRb.rotation, 
                                            Quaternion.Euler(new Vector3(0.0f, 0.0f, targetZRotation)), this.enemy.rotateSpeed);
    }

    private bool PlayerIsInSight()
    {
        Vector3 directionOfPlayer = (this.enemy.playerTarget.creatureRb.position - this.enemy.creatureRb.position).normalized;

        float angle = Vector3.Angle(this.enemy.gameObject.transform.right, directionOfPlayer);

        if (angle <= this.attackAngle)
        {
            return true;
        }

        return false;
    }

    public override void ExecuteBehavior()
    {
        this.TurnTowardsPlayer();
        
        if (this.PlayerIsInSight())
        {
            if (this.enemy.enemyAttackCoroutine == null)
            {
                this.enemy.attackCooldown = this.secondsBetweenAttacks;
                this.enemy.Attack();
            }
        }
    }
}
