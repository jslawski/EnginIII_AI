using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionStealWeaponBehavior : MinionEnemyBehavior
{    
    public override void ExecuteBehavior()
    {
        GameObject weaponInstance = this.enemy.CreateWeapon();
        this.enemy.carryingWeapon = weaponInstance.GetComponent<GrabbableWeapon>();
        this.enemy.carryingWeapon.Setup(this.enemy.playerTarget.equippedWeapon);
        this.enemy.carryingWeapon.grabbable = false;
        
        this.enemy.playerTarget.LoseWeapon();        
    }
}
