using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBuilding : Interactable
{
    public PlayerActions actions;
    public GameObject spawnMarker = null;
    public float maxMarkerDistance = 10f;

    public override void OnInteractEnter()
    {
        ActionFrame.instance.SetActionButtons(actions, spawnMarker);
        spawnMarker.SetActive(true);
        base.OnInteractEnter();
    }

    public override void OnInteractExit()
    {
        ActionFrame.instance.ClearButtons();
        spawnMarker.SetActive(false);
        base.OnInteractExit();
    }

    public void SetSpawnMarkerLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //make it better
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                spawnMarker.transform.position = hit.point;
            }
            else
            {
                Debug.Log("Invalid location! You can place spawn marker on empty ground.");
            }
        }
    }
}
