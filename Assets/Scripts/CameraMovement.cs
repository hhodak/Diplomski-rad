using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float speed = 0.05f;
    float zoomSpeed = 10f;
    float rotationSpeed = 0.2f;

    float minHeight = 2f;
    float maxHeight = 20f;

    Vector3 pos1;
    Vector3 pos2;

    Transform cameraChild;

    void Start()
    {
        cameraChild = transform.GetChild(0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 0.05f;
            zoomSpeed = 10f;
        }
        else
        {
            speed = 0.025f;
            zoomSpeed = 5f;
        }

        float horizontalSpeed = transform.position.y * speed * Input.GetAxis("Horizontal");
        float verticalSpeed = transform.position.y * speed * Input.GetAxis("Vertical");
        float scrollSpeed = Mathf.Log(transform.position.y) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        if (transform.position.y >= maxHeight && scrollSpeed > 0)
        {
            scrollSpeed = 0;
        }
        else if (transform.position.y <= minHeight && scrollSpeed < 0)
        {
            scrollSpeed = 0;
        }

        if (transform.position.y + scrollSpeed > maxHeight)
        {
            scrollSpeed = maxHeight - transform.position.y;
        }
        else if (transform.position.y + scrollSpeed < minHeight)
        {
            scrollSpeed = minHeight - transform.position.y;
        }

        Vector3 verticalMove = new Vector3(0, scrollSpeed, 0);
        Vector3 lateralMove = horizontalSpeed * transform.right;
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= verticalSpeed;

        Vector3 move = verticalMove + lateralMove + forwardMove;
        transform.position += move;

        GetCameraRotation();
    }

    void GetCameraRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            pos1 = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            pos2 = Input.mousePosition;
            float dx = (pos2 - pos1).x * rotationSpeed;
            float dy = (pos2 - pos1).y * rotationSpeed;
            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0));
            cameraChild.transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));
            pos1 = pos2;
        }
    }
}
