using System;
using UnityEngine;

public class BossEnemy : Enemy
{
    private Type CanSeePlayerBehavior = typeof(BossChaseBehavior);
    private Type CannotSeePlayerBehavior = typeof(BossIdleBehavior);
    private Type CanGrabWeaponBehavior = typeof(BossGrabWeaponBehavior);
    private Type CanAttackPlayerBehavior = typeof(BossAttackBehavior);

    private BossBehavior currentBehavior;

    protected override void Start()
    {
        base.Start();

        this.currentBehavior = new BossIdleBehavior();
        this.currentBehavior.Setup(this);
    }

    protected override void AttemptBehaviorChange(Type newBehaviorType)
    {
        if (this.currentBehavior.GetType() != newBehaviorType)
        {
            this.currentBehavior = Activator.CreateInstance(newBehaviorType, this) as BossBehavior;
            this.currentBehavior.Setup(this);
            this.currentBehaviorString = this.currentBehavior.GetType().ToString();
        }
    }

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

        this.currentBehavior.ExecuteBehavior();
    }

    protected override bool IsInRangeOfPlayer()
    {
        float distanceToWeaponMidpoint = Vector3.Distance(this.creatureRb.position, this.attackZone.gameObject.transform.position);
        float distanceToPlayer = Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position);

        return (distanceToPlayer <= distanceToWeaponMidpoint);
    }
}
