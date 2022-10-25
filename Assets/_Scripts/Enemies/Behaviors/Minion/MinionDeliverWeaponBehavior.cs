using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDeliverWeaponBehavior : MinionBehavior
{
    float minDeliveryDistance = 0.5f;

    public override void ExecuteBehavior()
    {
        this.minion.carryingWeapon.gameObject.transform.position =
            this.enemy.attackZone.gameObject.transform.position;

        if (this.CanDeliverWeapon())
        {
            this.DeliverWeapon();
        }
        else
        {
            this.MoveTowardsBoss();
        }
    }

    private bool CanDeliverWeapon()
    {
        return (Vector3.Distance(this.enemy.creatureRb.position, this.minion.boss.creatureRb.position) <= this.minDeliveryDistance);
    }

    private void MoveTowardsBoss()
    {
        Vector3 moveDirection = (this.minion.boss.creatureRb.position - this.enemy.creatureRb.position).normalized;
        float moveSpeedThisFrame = this.enemy.moveSpeed * Time.fixedDeltaTime;

        Vector3 targetDestination = this.enemy.creatureRb.position + (moveDirection * moveSpeedThisFrame);
        float targetZRotation = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        this.enemy.creatureRb.MovePosition(targetDestination);

        this.enemy.creatureRb.rotation = Quaternion.RotateTowards(this.enemy.creatureRb.rotation,
                                            Quaternion.Euler(new Vector3(0.0f, 0.0f, targetZRotation)), this.enemy.rotateSpeed);
    }

    private void DeliverWeapon()
    {
        if (this.minion.boss.weaponPickupCandidate == null)
        {
            this.minion.boss.weaponPickupCandidate = this.minion.carryingWeapon;
        }
    }
}
