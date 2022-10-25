using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDeliverWeaponBehavior : MinionBehavior
{
    public override void ExecuteBehavior()
    {
        this.minion.carryingWeapon.gameObject.transform.position =
            this.enemy.attackZone.gameObject.transform.position;


    }

    private void MoveTowardsBoss()
    {

    }


}
