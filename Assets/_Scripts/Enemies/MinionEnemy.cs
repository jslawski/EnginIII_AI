using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionEnemy : Enemy
{    
    private Type CanSeePlayerBehavior = typeof(ChaseBehavior);
    private Type CannotSeePlayerBehavior = typeof(IdleBehavior);
    private Type CanStealWeaponBehavior = typeof(AttackBehavior);
    private Type CanAttackPlayerBehavior = typeof(AttackBehavior);
    private Type IsHoldingWeaponBehavior = typeof(AttackBehavior);

    [Header("Minion Attributes")]
    [SerializeField]
    private float rangedAttackDistance = 8.0f;
    private float fleeDistance = 5.0f;

    private BossEnemy boss;

    [HideInInspector]
    public GrabbableWeapon carryingWeapon = null;

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
                    this.AttemptBehaviorChange(this.CanAttackPlayerBehavior);
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

    protected override bool IsInRangeOfPlayer()
    {
        float distanceToPlayer = Vector3.Distance(this.creatureRb.position, this.playerTarget.creatureRb.position);

        return (distanceToPlayer <= this.rangedAttackDistance);
    }    
}
