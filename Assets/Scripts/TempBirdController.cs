using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBirdController : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask groundMask;
    
    [Header("Info")]
    public bool isCursorConfined;
    public Vector3 cursorWorldPosition;

    Camera cam;

    void Update()
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        ManageCursorState();

        //dont update cursor position if we are not confined
        //alt tabbing breaks this
        if (!isCursorConfined)
        {
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, groundMask))
        {
            cursorWorldPosition = hit.point;

            Debug.DrawRay(hit.point, Vector3.up);
        }
    }

    void ManageCursorState()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.anyKeyDown)
        {
            if (!isCursorConfined)
            {
                Cursor.lockState = CursorLockMode.Confined;
                isCursorConfined = true;
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.lockState = CursorLockMode.None;
            isCursorConfined = false;
        }
    }
}
