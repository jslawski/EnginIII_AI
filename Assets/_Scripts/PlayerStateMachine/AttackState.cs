using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.creatureReference.Attack();        
    }

    public override void Exit()
    {
        base.Exit();        
    }

    public override void UpdateState()
    {
        //Don't allow players to pick up weapons or move while they're attacking
        //Transition to the idle state once the attack is completed
        if (this.controller.creatureReference.attackZone.attacking == false)
        {
            this.controller.ChangeState(new IdleState());
        }
    }    
}
