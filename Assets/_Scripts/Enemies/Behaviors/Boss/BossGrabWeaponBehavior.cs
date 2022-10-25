using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrabWeaponBehavior : BossEnemyBehavior
{    
    public override void ExecuteBehavior()
    {
        this.enemy.PickUpWeapon();
    }
}
