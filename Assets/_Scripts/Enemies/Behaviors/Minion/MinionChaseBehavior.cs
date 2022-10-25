using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionChaseBehavior : MinionBehavior
{
    public override void ExecuteBehavior()
    {
        Vector3 moveDirection = (this.enemy.playerTarget.creatureRb.position - this.enemy.creatureRb.position).normalized;
        float moveSpeedThisFrame = this.enemy.moveSpeed * Time.fixedDeltaTime;

        Vector3 targetDestination = this.enemy.creatureRb.position + (moveDirection * moveSpeedThisFrame);
        float targetZRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        this.enemy.creatureRb.MovePosition(targetDestination);
        //this.enemy.creatureRb.MoveRotation(Quaternion.Euler(new Vector3(0.0f, 0.0f, targetZRotation)));

        this.enemy.creatureRb.rotation = Quaternion.RotateTowards(this.enemy.creatureRb.rotation,
                                            Quaternion.Euler(new Vector3(0.0f, 0.0f, targetZRotation)), this.enemy.rotateSpeed);
    }
}
