using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public string unitName;
    public UnitClass unitClass;
    public float health;
    public float armor;
    public float cost;
    public float buildTime;
    public float sightRange;
    public float attackDamage;
    public float attackSpeed;
    public float attackRange;
    public float attackAnimationTime;
    public float aggroRange;

    public void MoveUnit()
    {
        //move unit
    }

    public void Attack()
    {
        //attack opponent
    }

    public void StopUnit()
    {
        //stop unit in position
    }
}
