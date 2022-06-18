using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStatTypes : ScriptableObject
{
    [System.Serializable]
    public class Base
    {
        public float duration, range, areaRadius, cost;
    }

    [System.Serializable]
    public class Healing
    {
        public float healingAmount;
    }
}
