using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionStealWeaponBehavior : MinionBehavior
{    
    public override void ExecuteBehavior()
    {
        GameObject weaponInstance = this.minion.CreateWeapon();
        this.minion.carryingWeapon = weaponInstance.GetComponent<GrabbableWeapon>();
        this.minion.carryingWeapon.Setup(this.enemy.playerTarget.equippedWeapon);
        this.minion.carryingWeapon.grabbable = false;
        
        this.enemy.playerTarget.LoseWeapon();        
    }
}
