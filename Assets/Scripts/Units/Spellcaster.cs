using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellcaster : MonoBehaviour
{
    public GameObject healingPrefab;

    public void HealUnit(Transform target)
    {

        GameObject heal = Instantiate(healingPrefab, target.position, Quaternion.identity, transform);
        Ability ability = heal.GetComponent<Ability>();
        UnitStatDisplay usd = transform.gameObject.GetComponentInChildren<UnitStatDisplay>();

        if (usd.currentEnergy >= ability.abilityType.baseStats.cost)
        {
            usd.ReduceEnergy(ability.abilityType.baseStats.cost);
            ability.HealTarget(target);
        }
        else
        {
            StartCoroutine(LogController.instance.ShowMessage("Not enough energy!"));
            Destroy(heal);
        }
    }
}
