using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestManager : MonoBehaviour
{
    void Awake()
    {
        GameObject temp = new GameObject();
        for (int i = 8; i <= 9; i++)
        {
            GameObject player = Instantiate(temp, this.transform.position, this.transform.rotation, this.transform);
            player.name = LayerMask.LayerToName(i);
            foreach (UnitClass unitClass in Enum.GetValues(typeof(UnitClass)))
            {
                GameObject child = Instantiate(temp, this.transform.position, this.transform.rotation, player.transform);
                child.name = unitClass.ToString();
            }
        }
        Destroy(temp);
    }
}
