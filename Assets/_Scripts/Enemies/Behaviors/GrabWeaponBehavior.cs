using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabWeaponBehavior : EnemyBehavior
{    
    public override void ExecuteBehavior()
    {
        this.enemy.PickUpWeapon();
    }
}
