using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionChaseBehavior : MinionBehavior
{
    public override void ExecuteBehavior()
    {
        this.minion.UpdateTargetDestination();

        Vector3 moveDirection = (this.minion.targetDestination - this.enemy.creatureRb.position).normalized;
        float moveSpeedThisFrame = this.enemy.moveSpeed * Time.fixedDeltaTime;

        Vector3 nextFramePosition = this.enemy.creatureRb.position + (moveDirection * moveSpeedThisFrame);
        float targetZRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        this.enemy.creatureRb.MovePosition(nextFramePosition);

        this.enemy.creatureRb.rotation = Quaternion.RotateTowards(this.enemy.creatureRb.rotation,
                                            Quaternion.Euler(new Vector3(0.0f, 0.0f, targetZRotation)), this.enemy.rotateSpeed);
    }
}
