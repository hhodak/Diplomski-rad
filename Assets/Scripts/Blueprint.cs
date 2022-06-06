using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    RaycastHit hit;
    public GameObject prefab;
    public Material blueprintMaterial;
    public Material blueprintInvalidMaterial;
    BuildingPlacement buildingPlacement;
    Renderer renderer;

    void Start()
    {
        buildingPlacement = GetComponent<BuildingPlacement>();
        renderer = GetComponent<Renderer>();
        StickBlueprintToMouse();
        InputHandler.instance.isBuildingProcess = true;
    }

    void Update()
    {
        StickBlueprintToMouse();

        if (CanPlaceBuilding())
        {
            SetBlueprintMaterial(blueprintMaterial);
        }
        else
        {
            SetBlueprintMaterial(blueprintInvalidMaterial);
        }

        if (Input.GetMouseButton(0))
        {
            if (CanPlaceBuilding())
            {
                Vector3 position = new Vector3(transform.position.x, prefab.transform.position.y, transform.position.z);
                GameObject building = Instantiate(prefab, position, transform.rotation);
                ActionFrame.instance.SetBuildingParent(building);
                InputHandler.instance.isBuildingProcess = false;
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Invalid location!");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            InputHandler.instance.isBuildingProcess = false;
            Destroy(gameObject);
        }
    }

    void StickBlueprintToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 7)))
        {
            transform.position = new Vector3(hit.point.x, prefab.transform.position.y, hit.point.z);
        }
    }

    bool CanPlaceBuilding()
    {
        if (buildingPlacement.colliders.Count > 0)
            return false;
        return true;
    }

    void SetBlueprintMaterial(Material material)
    {
        renderer.material = material;
    }
}
