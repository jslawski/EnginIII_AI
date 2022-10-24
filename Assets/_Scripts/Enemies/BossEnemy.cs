using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    private Type CanSeePlayerBehavior = typeof(ChaseBehavior);
    private Type CannotSeePlayerBehavior = typeof(IdleBehavior);
    private Type CanGrabWeaponBehavior = typeof(GrabWeaponBehavior);    

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
                this.AttemptBehaviorChange(this.CanSeePlayerBehavior);
            }
            else
            {
                this.AttemptBehaviorChange(this.CannotSeePlayerBehavior);
            }
        }
    }
}
