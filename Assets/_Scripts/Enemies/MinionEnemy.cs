using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemy : Enemy
{    
    private Type CanSeePlayerBehavior = typeof(ChaseBehavior);
    private Type CannotSeePlayerBehavior = typeof(IdleBehavior);
    private Type CanStealWeaponBehavior = typeof(MinionStealWeaponBehavior);
    private Type CanAttackPlayerBehavior = typeof(AttackBehavior);
    private Type IsHoldingWeaponBehavior = typeof(MinionDeliverWeaponBehavior);
    private Type PlayerTooCloseBehavior = typeof(AttackBehavior);  //Change this to MinionFleeBehavior if you want the minion to flee

    [Header("Minion Attributes")]
    [SerializeField]
    private float rangedAttackDistance = 8.0f;
    [SerializeField]
    private float fleeDistance = 4.0f;

    private BossEnemy boss;

    private GameObject projectilePrefab;

    protected override void Start()
    {
        base.Start();

        this.boss = GameObject.Find("BossEnemy").GetComponent<BossEnemy>();
        this.projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
    }

    protected override void UpdateBehavior()
    {
        if (this.carryingWeapon != null)
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
                        this.AttemptBehaviorChange(this.CanSeePlayerBehavior);
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
        Debug.LogError("Weapon Utilities:   Boss: " + this.boss.equippedWeapon.GetUtilityTotalScore() +
            "   Player: " + this.playerTarget.equippedWeapon.GetUtilityTotalScore());

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
        float distanceToPlayer = Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position);

        return (distanceToPlayer <= this.rangedAttackDistance);
    }    
}
