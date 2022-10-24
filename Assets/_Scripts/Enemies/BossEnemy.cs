using System;
using UnityEngine;

public class BossEnemy : Enemy
{
    private Type CanSeePlayerBehavior = typeof(ChaseBehavior);
    private Type CannotSeePlayerBehavior = typeof(IdleBehavior);
    private Type CanGrabWeaponBehavior = typeof(BossGrabWeaponBehavior);
    private Type CanAttackPlayerBehavior = typeof(AttackBehavior);
    
    protected override void UpdateBehavior()
    {        
        if (this.weaponPickupCandidate != null)
        {
            this.AttemptBehaviorChange(this.CanGrabWeaponBehavior);
        }
        else
        {
            if (this.CanSeePlayer()  == true)
            {
                if (this.IsInRangeOfPlayer() == true)
                {
                    this.AttemptBehaviorChange(this.CanAttackPlayerBehavior);
                }
                else
                {
                    this.AttemptBehaviorChange(this.CanSeePlayerBehavior);
                }
            }
            else
            {
                this.AttemptBehaviorChange(this.CannotSeePlayerBehavior);
            }
        }
    }

    protected override bool IsInRangeOfPlayer()
    {
        float distanceToWeaponMidpoint = Vector3.Distance(this.creatureRb.position, this.attackZone.gameObject.transform.position);
        float distanceToPlayer = Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position);

        return (distanceToPlayer <= distanceToWeaponMidpoint);
    }
}
