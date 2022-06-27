using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;
    RaycastHit hit;
    public List<Transform> selectedUnits = new List<Transform>();
    public Transform selectedBuilding = null;
    bool isDragging = false;
    Vector3 mousePosition;
    public LayerMask interactableLayer = new LayerMask();
    bool isGamePause = false;
    public GameObject pausePanel;
    public bool isBuildingProcess = false;

    List<Transform>[] hotkey = new List<Transform>[10];

    void Awake()
    {
        instance = this;
        InitializeHotkeyLists();
    }

    private void OnGUI()
    {
        if (isDragging)
        {
            Rect rect = MultiSelect.GetRectangle(mousePosition, Input.mousePosition);
            MultiSelect.DrawRectangle(rect, new Color(0f, 1f, 0f, 0.25f));
            MultiSelect.DrawRectangleBorder(rect, 2, new Color(0f, 1f, 0f, 1f));
        }
    }

    public void HandleUserInput()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause(false);
        }

        if (!isGamePause)
        {
            if (!isBuildingProcess)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    mousePosition = Input.mousePosition;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //get camera from somewhere else (from variable)
                    if (Physics.Raycast(ray, out hit, 1000, interactableLayer))
                    {
                        if (AddedUnit(hit.transform, Input.GetKey(KeyCode.LeftControl)))
                        {
                            //be able to do stuff with units
                        }
                        else if (AddedBuilding(hit.transform))
                        {
                            //be able to do stuff with bullding
                        }
                    }
                    else
                    {
                        isDragging = true;
                        DeselectUnits();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    foreach (Transform child in PlayerManager.instance.playerUnits)
                    {
                        foreach (Transform unit in child)
                        {
                            if (IsWithinSelectionBounds(unit))
                            {
                                AddedUnit(unit, true);
                            }
                        }
                    }
                    isDragging = false;
                }

                if (Input.GetMouseButtonDown(1) && HaveSelectedUnits())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //get camera from somewhere else (from variable)
                    if (Physics.Raycast(ray, out hit))
                    {
                        //FIND BETTER SOLUTION
                        foreach (Transform unit in selectedUnits)
                        {
                            PlayerUnit playerUnit = unit.gameObject.GetComponent<PlayerUnit>();
                            if (playerUnit.unitType.unitType == BasicUnit.UnitType.Worker)
                            {
                                playerUnit.GetComponent<Worker>().HandleWorking(false);
                            }
                        }
                        //
                        LayerMask layerHit = hit.transform.gameObject.layer;
                        switch (layerHit.value)
                        {
                            case 8://Units layer
                                   //heal
                                if (selectedUnits.Count == 1 && selectedUnits[0].GetComponent<Spellcaster>() != null)
                                {
                                    selectedUnits[0].GetComponent<Spellcaster>().HealUnit(hit.transform);
                                }
                                break;
                            case 9://Enemy units layer
                                foreach (Transform unit in selectedUnits)
                                {
                                    PlayerUnit playerUnit = unit.gameObject.GetComponent<PlayerUnit>();
                                    playerUnit.SetEnemyTarget(hit.transform);
                                }
                                break;
                            case 10:
                                //gather resources
                                foreach (Transform unit in selectedUnits)
                                {
                                    PlayerUnit playerUnit = unit.gameObject.GetComponent<PlayerUnit>();
                                    if (playerUnit.unitType.unitType == BasicUnit.UnitType.Worker)
                                    {
                                        playerUnit.GetComponent<Worker>().HandleWorking(true, hit.transform);
                                    }
                                }
                                break;
                            default:
                                foreach (Transform unit in selectedUnits)
                                {
                                    PlayerUnit playerUnit = unit.gameObject.GetComponent<PlayerUnit>();
                                    playerUnit.hasAggro = false;
                                    playerUnit.MoveUnit(hit.point);
                                }
                                break;
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(1) && selectedBuilding != null)
                {
                    selectedBuilding.gameObject.GetComponent<IBuilding>().SetSpawnMarkerLocation();
                }

                #region Hotkeys
                if (Input.inputString != "")
                {
                    int number; //set default value -1
                    bool isNumber = int.TryParse(Input.inputString, out number);
                    if (isNumber && number >= 0 && number <= 9)
                    {
                        if (Input.GetKey(number.ToString()) && Input.GetKey(KeyCode.Q)) //Q -> LeftControl
                        {
                            AddHotkeyUnits(number);
                        }
                        else if (Input.GetKey(number.ToString()) && Input.GetKey(KeyCode.E)) //E -> LeftShift
                        {
                            RemoveHotkeyUnits(number);
                        }
                        else if (Input.GetKeyDown(number.ToString()))
                        {
                            SelectHotkeyUnits(number);
                        }
                    }
                }
                #endregion
            }
        }
    }

    void DeselectUnits()
    {
        if (selectedBuilding)
        {
            selectedBuilding.gameObject.GetComponent<IBuilding>().OnInteractExit();
            selectedBuilding = null;
        }
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].gameObject.GetComponent<IUnit>().OnInteractExit();
        }
        selectedUnits.Clear();
    }

    bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }

        Camera camera = Camera.main; //better fixes??
        Bounds viewportBounds = MultiSelect.GetViewportBounds(camera, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }

    bool HaveSelectedUnits()
    {
        if (selectedUnits.Count > 0)
        {
            return true;
        }
        return false;
    }

    IUnit AddedUnit(Transform tf, bool canMultiselect = false)
    {
        IUnit iUnit = tf.GetComponent<IUnit>();
        if (iUnit)
        {
            if (!canMultiselect)
            {
                DeselectUnits();
            }
            selectedUnits.Add(iUnit.gameObject.transform);
            iUnit.OnInteractEnter();
            return iUnit;
        }
        return null;
    }

    IBuilding AddedBuilding(Transform tf)
    {
        IBuilding iBuilding = tf.GetComponent<IBuilding>();
        if (iBuilding)
        {
            DeselectUnits();

            selectedBuilding = iBuilding.gameObject.transform;
            iBuilding.OnInteractEnter();
            return iBuilding;
        }
        return null;
    }

    void InitializeHotkeyLists()
    {
        for (int i = 0; i < 10; i++)
        {
            hotkey[i] = new List<Transform>();
        }
    }

    void SelectHotkeyUnits(int num)
    {
        DeselectUnits();
        for (int i = 0; i < hotkey[num].Count; i++)
        {
            AddedUnit(hotkey[num][i], true);
        }
    }

    void AddHotkeyUnits(int num)
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (!hotkey[num].Contains(selectedUnits[i]))
            {
                hotkey[num].Add(selectedUnits[i]);
            }
        }
    }

    void RemoveHotkeyUnits(int num)
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (hotkey[num].Contains(selectedUnits[i]))
            {
                hotkey[num].Remove(selectedUnits[i]);
            }
        }
    }

    public void RemoveDestroyedUnitFromHotkey(Transform unit)
    {
        for (int i = 0; i < 10; i++)
        {
            if (hotkey[i].Contains(unit))
            {
                hotkey[i].Remove(unit);
            }
        }
    }

    public void GamePause(bool isGameEnd)
    {
        Time.timeScale = isGamePause.GetHashCode();
        isGamePause = !isGamePause;
        if (!isGameEnd)
            pausePanel.SetActive(isGamePause);
    }
}
