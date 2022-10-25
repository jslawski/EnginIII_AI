using System;
using System.Collections;
using UnityEngine;

public class Enemy : Creature
{    
    [HideInInspector]
    public Creature playerTarget;

    [Header("Enemy Attributes")]
    public float visionDistance = 15f;
    public float rotateSpeed = 3.0f;    

    [HideInInspector]
    public float attackCooldown = 0.0f;

    [HideInInspector]
    public Coroutine enemyAttackCoroutine = null;

    public float attackAngle = 30.0f;
    public float secondsBetweenAttacks = 2.0f;
    
    public string currentBehaviorString;

    protected override void Start()
    {
        base.Start();

        this.playerTarget = GameObject.Find("Player").GetComponent<Creature>();        
    }

    private void FixedUpdate()
    {
        //Enemy should be motionless while it is attacking
        if (this.currentAttack != null)
        {
            return;
        }

        this.UpdateBehavior();                
    }

    protected virtual void UpdateBehavior() { }

    protected virtual void AttemptBehaviorChange(Type newBehaviorType) { }

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

    public void TurnTowardsPlayer()
    {
        Vector3 targetDirection = (this.playerTarget.creatureRb.position - this.creatureRb.position).normalized;
        float targetZRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        this.creatureRb.rotation = Quaternion.RotateTowards(this.creatureRb.rotation,
                                   Quaternion.Euler(new Vector3(0.0f, 0.0f, targetZRotation)), this.rotateSpeed);
    }

    public bool PlayerIsInSight()
    {
        Vector3 directionOfPlayer = (this.playerTarget.creatureRb.position - this.creatureRb.position).normalized;

        float angle = Vector3.Angle(this.gameObject.transform.right, directionOfPlayer);

        if (angle <= this.attackAngle)
        {
            return true;
        }

        return false;
    }

    protected IEnumerator DepleteAttackCooldown()
    {
        while (this.attackCooldown >= 0.0f)
        {
            this.attackCooldown -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        this.enemyAttackCoroutine = null;
    }    

    /*
    public override void TakeDamage(Creature attackingCreature, int damage)
    {
        base.TakeDamage(attackingCreature, damage);
        StartCoroutine(this.HaltMovement());
    }

    private IEnumerator HaltMovement()
    {
        this.creatureRb.constraints = RigidbodyConstraints.FreezeAll;

        yield return new WaitForSeconds(0.5f);

        this.creatureRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }
    */
}
