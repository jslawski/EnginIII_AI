using System;
using System.Collections;
using UnityEngine;

public class Enemy : Creature
{
    public string currentBehaviorString;

    [HideInInspector]
    public Creature playerTarget;
    protected EnemyBehavior currentBehavior;    
    protected float visionDistance = 15f;

    public float rotateSpeed = 3.0f;
    public float attackCooldown = 0.0f;

    [HideInInspector]
    public Coroutine enemyAttackCoroutine = null;

    protected override void Start()
    {
        base.Start();

        this.playerTarget = GameObject.Find("Player").GetComponent<Creature>();
        this.currentBehavior = new IdleBehavior();
        this.currentBehavior.Setup(this);
    }

    private void FixedUpdate()
    {
        //Enemy should be motionless while it is attacking
        if (this.currentAttack != null)
        {
            return;
        }

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
            this.currentBehaviorString = this.currentBehavior.GetType().ToString();
        }
    }

    protected bool CanSeePlayer()
    {
        return (Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position) <= this.visionDistance);
    }

    protected virtual bool IsInRangeOfPlayer() { return false; }

    public override void Attack()
    {
        if (this.enemyAttackCoroutine != null)
        {
            return;
        }

        base.Attack();
        this.enemyAttackCoroutine = StartCoroutine(this.DepleteAttackCooldown());
    }

    private IEnumerator DepleteAttackCooldown()
    {
        while (this.attackCooldown >= 0.0f)
        {
            this.attackCooldown -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        this.enemyAttackCoroutine = null;
    }
}
