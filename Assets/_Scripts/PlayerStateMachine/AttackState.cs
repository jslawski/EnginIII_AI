using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : PlayerState
{
    private float currentSwingTimeInSeconds = 0.0f;
    private float swingTimeInSeconds = 0.5f;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.creatureReference.attackZone.EnableAttack();
        this.controller.creatureReference.TriggerAttack();
    }

    public override void Exit()
    {
        base.Exit();

        this.controller.creatureReference.attackZone.DisableAttack();
    }

    public override void UpdateState()
    {
        //Don't allow players to pick up weapons while they're attacking
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this.currentSwingTimeInSeconds < this.swingTimeInSeconds)
        {
            this.currentSwingTimeInSeconds += Time.fixedDeltaTime;
        }
        else
        {
            this.controller.ChangeState(new IdleState());
        }
    }
}
