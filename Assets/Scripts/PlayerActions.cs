using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Action", menuName = "Player Action")]
public class PlayerActions : ScriptableObject
{
    [Header("Units")]
    public List<BasicUnit> basicUnits = new List<BasicUnit>();

    [Header("Buildings")]
    public List<BasicBuilding> basicBuildings = new List<BasicBuilding>();

    [Header("Abilities")]
    public List<BasicAbility> basicAbilities = new List<BasicAbility>();
}