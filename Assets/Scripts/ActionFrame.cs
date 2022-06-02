using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionFrame : MonoBehaviour
{
    public static ActionFrame instance = null;

    [SerializeField] private Button actionButton = null;
    [SerializeField] private Transform layoutGroup = null;

    List<Button> buttons = new List<Button>();
    [HideInInspector]
    public PlayerActions actionsList = null;
    public List<float> spawnQueue = new List<float>();
    public List<GameObject> spawnOrder = new List<GameObject>();

    public GameObject spawnPoint = null;

    private void Awake()
    {
        instance = this;
    }

    public void SetActionButtons(PlayerActions actions, GameObject spawnLocation)
    {
        actionsList = actions;
        spawnPoint = spawnLocation;

        if (actions != null && actions.basicUnits.Count > 0)
        {
            foreach (BasicUnit unit in actions.basicUnits)
            {
                Button btn = Instantiate(actionButton, layoutGroup);
                btn.name = unit.unitName;
                GameObject icon = Instantiate(unit.icon, btn.transform);
                //add text, image...
                buttons.Add(btn);
            }
        }
        if (actions != null && actions.basicBuildings.Count > 0 && InputHandler.instance.selectedUnits.Count == 1)
        {
            foreach (BasicBuilding building in actions.basicBuildings)
            {
                Button btn = Instantiate(actionButton, layoutGroup);
                btn.name = building.buildingName;
                GameObject icon = Instantiate(building.icon, btn.transform);
                //add text, image...
                buttons.Add(btn);
            }
        }
    }

    public void ClearButtons()
    {
        foreach (Button btn in buttons)
        {
            Destroy(btn.gameObject);
        }
        buttons.Clear();
    }

    public void StartSpawnTimer(string objectName)
    {
        if (IsUnit(objectName))
        {
            BasicUnit unit = IsUnit(objectName);
            PlayerBuilding selectedBuilding = InputHandler.instance.selectedBuilding.GetComponent<PlayerBuilding>();
            selectedBuilding.SpawnUnit(unit);
        }
        else if (IsBuilding(objectName))
        {
            BasicBuilding building = IsBuilding(objectName);
            PlayerUnit unit = InputHandler.instance.selectedUnits[0].GetComponent<PlayerUnit>();
            unit.GetComponent<Worker>().SpawnBuilding(building);
        }
        else
        {
            Debug.Log($"{objectName} is not a spawnable object!");
        }
    }

    BasicUnit IsUnit(string name)
    {
        if (actionsList.basicUnits.Count > 0)
        {
            foreach (BasicUnit unit in actionsList.basicUnits)
            {
                if (unit.unitName == name)
                {
                    return unit;
                }
            }
        }
        return null;
    }

    BasicBuilding IsBuilding(string name)
    {
        if (actionsList.basicBuildings.Count > 0)
        {
            foreach (BasicBuilding building in actionsList.basicBuildings)
            {
                if (building.buildingName == name)
                {
                    return building;
                }
            }
        }
        return null;
    }

    public void SetBuildingParent(GameObject building)
    {
        PlayerBuilding playerBuilding = building.GetComponent<PlayerBuilding>();
        playerBuilding.transform.SetParent(GameObject.Find("Player" + playerBuilding.buildingType.buildingType.ToString()).transform);
    }
}
