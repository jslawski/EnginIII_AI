using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemy : Enemy
{    
    private Type CanSeePlayerBehavior = typeof(MinionChaseBehavior);
    private Type CannotSeePlayerBehavior = typeof(MinionIdleBehavior);
    private Type CanSeeWeaponBehavior = typeof(MinionApproachBehavior);
    private Type CanStealWeaponBehavior = typeof(MinionStealWeaponBehavior);
    private Type CanAttackPlayerBehavior = typeof(MinionAttackBehavior);
    private Type IsHoldingWeaponBehavior = typeof(MinionDeliverWeaponBehavior);
    private Type PlayerTooCloseBehavior = typeof(MinionAttackBehavior);  //Change this to MinionFleeBehavior if you want the minion to flee

    [Header("Minion Attributes")]
    [SerializeField]
    private float rangedAttackDistance = 0.1f;
    [SerializeField]
    private float fleeDistance = 0.0f;

    [HideInInspector]
    public GrabbableWeapon carryingWeapon = null;

    [HideInInspector]
    public BossEnemy boss;

    [HideInInspector]
    public Vector3 targetDestination;

    [SerializeField]
    private float flankDistance = 1.0f;

    private GameObject projectilePrefab;

    private MinionBehavior currentBehavior;

    protected override void Start()
    {
        base.Start();

        this.boss = GameObject.Find("BossEnemy").GetComponent<BossEnemy>();
        this.projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");

        this.currentBehavior = new MinionIdleBehavior();
        this.currentBehavior.Setup(this);
    }

    protected override void AttemptBehaviorChange(Type newBehaviorType)
    {
        if (this.currentBehavior.GetType() != newBehaviorType)
        {
            this.currentBehavior = Activator.CreateInstance(newBehaviorType) as MinionBehavior;
            this.currentBehavior.Setup(this);
            this.currentBehaviorString = this.currentBehavior.GetType().ToString();
        }
    }

    protected override void UpdateBehavior()
    {
        if (this.carryingWeapon != null && this.boss != null)
        {
            this.AttemptBehaviorChange(this.IsHoldingWeaponBehavior);
        }
        else
        {
            if (this.CanSeePlayer() == true)
            {
                if (this.ValuableWeaponDetected() && this.boss != null)
                {
                    if (this.IsInStealingRange())
                    {
                        this.AttemptBehaviorChange(this.CanStealWeaponBehavior);
                    }
                    else
                    {
                        this.AttemptBehaviorChange(this.CanSeeWeaponBehavior);
                    }
                }
                else if (this.IsInRangeOfPlayer() == true)
                {
                    if (this.ShouldFlee())
                    {
                        this.AttemptBehaviorChange(this.PlayerTooCloseBehavior);
                    }
                    else
                    {
                        this.AttemptBehaviorChange(this.CanAttackPlayerBehavior);
                    }
                }
                else
                {
                    this.AttemptBehaviorChange(this.CanSeePlayerBehavior);
                }
            }
            else
            {
                this.AttemptBehaviorChange(this.CannotSeePlayerBehavior);
            }
        }

        this.currentBehavior.ExecuteBehavior();
    }

    public override void Attack()
    {
        Vector3 directionTowardsPlayer = (this.playerTarget.creatureRb.position - this.creatureRb.position).normalized;

        GameObject projectileInstance = Instantiate(this.projectilePrefab, this.creatureRb.position, new Quaternion());
        Projectile projectileComponent = projectileInstance.GetComponent<Projectile>();
        projectileComponent.Launch(this, directionTowardsPlayer);

        this.enemyAttackCoroutine = StartCoroutine(this.DepleteAttackCooldown());

        this.TriggerAttack();
    }

    private bool ValuableWeaponDetected()
    {
        //Debug.LogError("Weapon Utilities:   Boss: " + this.boss.equippedWeapon.GetUtilityTotalScore() +
         //   "   Player: " + this.playerTarget.equippedWeapon.GetUtilityTotalScore());

        if (this.playerTarget.equippedWeapon == this.unarmedWeapon)
        {
            return false;
        }

        return (this.playerTarget.equippedWeapon.GetUtilityTotalScore() > 
            this.boss.equippedWeapon.GetUtilityTotalScore());
    }

    private bool IsInStealingRange()
    {
        float distanceToWeaponMidpoint = Vector3.Distance(this.creatureRb.position, this.attackZone.gameObject.transform.position);
        float distanceToPlayer = Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position);

        return (distanceToPlayer <= distanceToWeaponMidpoint);
    }
    
    private bool ShouldFlee()
    {
        float distanceToPlayer = Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position);
        return (distanceToPlayer <= this.fleeDistance);        
    }

    protected override bool IsInRangeOfPlayer()
    {
        this.UpdateTargetDestination();
        float distanceToTarget = Vector3.Distance(this.targetDestination, this.creatureRb.position);

        return (distanceToTarget <= this.rangedAttackDistance);
    }

    public GameObject CreateWeapon()
    {
        return Instantiate(Resources.Load<GameObject>("Prefabs/GrabbableWeapon"), this.creatureRb.position, new Quaternion());
    }

    protected override void Die()
    {
        if (this.carryingWeapon != null)
        {
            this.carryingWeapon.grabbable = true;
        }

        base.Die();
    }

    public void UpdateTargetDestination()
    {
        Vector3 targetDestinationLeft = this.playerTarget.creatureRb.position +
            (this.playerTarget.transform.up * this.flankDistance);

        Vector3 targetDestinationRight = this.playerTarget.creatureRb.position -
            (this.playerTarget.transform.up * this.flankDistance);

        Vector3 targetDestination = Vector3.zero;

        if (Vector3.Distance(this.creatureRb.position, targetDestinationLeft) <
            Vector3.Distance(this.creatureRb.position, targetDestinationRight))
        {
            this.targetDestination = targetDestinationLeft;
        }
        else
        {
            this.targetDestination = targetDestinationRight;
        }
    }
}
