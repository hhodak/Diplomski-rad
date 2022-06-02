using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatTypes : ScriptableObject
{
    [System.Serializable]
    public class Base
    {
        public float health, armor, movementSpeed, damage, attackSpeed, attackRange, aggroRange, energy, cost;
    }
}
