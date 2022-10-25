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
    public GrabbableWeapon carryingWeapon = null;

    [HideInInspector]
    public float attackCooldown = 0.0f;

    [HideInInspector]
    public Coroutine enemyAttackCoroutine = null;

    protected EnemyBehavior currentBehavior;

    public float attackAngle = 30.0f;
    public float secondsBetweenAttacks = 2.0f;
    
    public string currentBehaviorString;

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

    public GameObject CreateWeapon()
    {
        return Instantiate(Resources.Load<GameObject>("Prefabs/GrabbableWeapon"), this.creatureRb.position, new Quaternion());
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
