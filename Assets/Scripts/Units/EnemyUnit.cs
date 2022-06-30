using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUnit : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public BasicUnit unitType;
    public UnitStatTypes.Base baseStats;
    public UnitStatTypes.Siege siegeStats;
    public UnitStatDisplay statDisplay;

    Collider[] rangeColliders;
    Transform aggroTarget;
    UnitStatDisplay aggroUnit;
    bool hasAggro = false;
    public float distance;
    public float attackCooldown;

    private void Start()
    {
        baseStats = unitType.baseStats;
        siegeStats = unitType.siegeStats;
        statDisplay.SetStatDisplayBasicUnit(baseStats, false);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = baseStats.movementSpeed;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        if (!hasAggro)
        {
            CheckForEnemyTargets();
        }
        else
        {
            SetTargetDistance();
            Attack();
            MoveToAggroTarget();
        }
    }

    public void MoveUnit(Vector3 destination)
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        navMeshAgent.stoppingDistance = 0;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(destination);
    }

    public void StopUnit()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        navMeshAgent.stoppingDistance = 0;
        navMeshAgent.isStopped = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void CheckForEnemyTargets()
    {
        rangeColliders = Physics.OverlapSphere(transform.position, baseStats.aggroRange, UnitHandler.instance.playerUnitLayer);

        for (int i = 0; i < rangeColliders.Length;)
        {
            aggroTarget = rangeColliders[i].gameObject.transform;
            aggroUnit = aggroTarget.gameObject.GetComponentInChildren<UnitStatDisplay>();
            hasAggro = true;
            break;
        }
    }

    void MoveToAggroTarget()
    {
        if (aggroTarget == null)
        {
            navMeshAgent.SetDestination(transform.position);
            hasAggro = false;
        }
        else
        {
            SetTargetDistance();
            navMeshAgent.stoppingDistance = baseStats.attackRange + 1;
            if (distance >= baseStats.aggroRange)
            {
                navMeshAgent.SetDestination(aggroTarget.position);
            }
        }
    }

    void Attack()
    {
        if (attackCooldown <= 0 && distance <= baseStats.attackRange + 1)
        {
            DoAttackAnimation();
            aggroUnit.TakeDamage(baseStats.damage);
            attackCooldown = baseStats.attackSpeed;
        }
    }

    void DoAttackAnimation()
    {
        switch (unitType.unitType)
        {
            case BasicUnit.UnitType.Melee:
                Melee melee = GetComponent<Melee>();
                melee.MeleeAttack();
                break;
            case BasicUnit.UnitType.Ranged:
                Ranged ranged = GetComponent<Ranged>();
                ranged.RangedAttack(aggroTarget, baseStats.damage);
                break;
            case BasicUnit.UnitType.Siege:
                Siege siege = GetComponent<Siege>();
                siege.SiegeAttack(aggroTarget, siegeStats.splashDamage);
                break;
            case BasicUnit.UnitType.Spellcaster:
                Debug.Log("Spellcaster animation not implemented yet.");
                break;
            case BasicUnit.UnitType.Worker:
                Debug.Log("Worker animation not implemented yet.");
                break;

            default: break;
        }
    }

    void SetTargetDistance()
    {
        if (hasAggro && aggroTarget)
        {
            distance = Vector3.Distance(aggroTarget.position, transform.position);
        }
    }
}
