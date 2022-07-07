using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsText : MonoBehaviour
{
    private void OnEnable()
    {
        string value = MainMenu.instance.GetPropValueToString(transform.name);
        GetComponent<TMPro.TextMeshProUGUI>().text = value;
    }
}
