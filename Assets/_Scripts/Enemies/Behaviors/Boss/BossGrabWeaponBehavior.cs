using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrabWeaponBehavior : BossBehavior
{    
    public override void ExecuteBehavior()
    {
        this.enemy.PickUpWeapon();
        this.enemy.weaponPickupCandidate = null;
    }
}
