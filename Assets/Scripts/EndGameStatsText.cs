using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameStatsText : MonoBehaviour
{
    private void Start()
    {
        string value = GameManager.instance.GetPropValueToString(transform.name);
        GetComponent<TMPro.TextMeshProUGUI>().text = value;
    }
}