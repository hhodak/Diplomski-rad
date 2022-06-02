using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    RaycastHit hit;
    public GameObject prefab;

    void Start()
    {
        StickBlueprintToMouse();
    }

    void Update()
    {
        StickBlueprintToMouse();

        if (Input.GetMouseButton(0))
        {
            Vector3 position = new Vector3(transform.position.x, prefab.transform.position.y, transform.position.z);
            GameObject building = Instantiate(prefab, position, transform.rotation);
            ActionFrame.instance.SetBuildingParent(building);
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
}
