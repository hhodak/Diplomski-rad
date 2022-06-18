using UnityEngine;

public class BuildingStatTypes : ScriptableObject
{
    [System.Serializable]
    public class Base
    {
        public float health;
        public float armor;
        public float cost;
    }
}
