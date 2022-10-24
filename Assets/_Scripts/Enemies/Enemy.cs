using System;
using UnityEngine;

public class Enemy : Creature
{
    [HideInInspector]
    public Creature playerTarget;
    protected EnemyBehavior currentBehavior;    
    protected float visionDistance = 8f;

    protected override void Start()
    {
        base.Start();

        this.playerTarget = GameObject.Find("Player").GetComponent<Creature>();
        this.currentBehavior = new IdleBehavior();
        this.currentBehavior.Setup(this);
    }

    private void FixedUpdate()
    {
        this.UpdateBehavior();
        this.currentBehavior.ExecuteBehavior();
    }

    protected virtual void UpdateBehavior() { }

    protected void AttemptBehaviorChange(Type newBehaviorType)
    {
        if (this.currentBehavior.GetType() != newBehaviorType)
        {
            this.currentBehavior = Activator.CreateInstance(newBehaviorType, this) as EnemyBehavior;
            this.currentBehavior.Setup(this);
        }
    }

    protected bool CanSeePlayer()
    {
        return (Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position) <= this.visionDistance);
    }

    protected virtual bool IsInRangeOfPlayer() { return false; }
}
