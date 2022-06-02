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
            aggroUnit.TakeDamage(baseStats.damage);
            attackCooldown = baseStats.attackSpeed;
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
