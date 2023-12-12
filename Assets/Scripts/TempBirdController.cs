using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBirdController : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask groundMask;
    
    [Header("Info")]
    public bool isCursorConfined;
    public Vector3 cursorWorldPosition = new Vector3(0f, 0f, 0f);

    Camera cam;

    [Header("Birds")]

    public GameObject birdRoot;

    public GameObject COM;

    public List<Rigidbody> boids;

    // Flock size
    public int num_boids = 1;

    // Food_points: Food: Food_points/10 -> 2/10 
    public int food_points = 0;

    public Vector3 com;

    public ScoreUI scoreUI;

    void Update()
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        // ManageCursorState();

        // //dont update cursor position if we are not confined
        // //alt tabbing breaks this
        // if (!isCursorConfined)
        // {
        //     return;
        // }
        
        // Outer if statement makes it mouse-click instead of continuous controls
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, groundMask))
            {
                cursorWorldPosition = hit.point;
                // Debug.DrawRay(hit.point, Vector3.up);
            }
        }

        boids = new List<Rigidbody>(); // Initialize followers list
        num_boids = birdRoot.transform.childCount;
        com = new Vector3(0f, 0f, 0f);
        if (num_boids == 0) {
            scoreUI.SetGameOverText();
        }

        for (int i = 0; i < num_boids; i++) {
            Rigidbody boid = birdRoot.transform.GetChild(i).gameObject.GetComponent<Rigidbody>();
            if (boid != null) {
                com += boid.position;
                boids.Add(boid);
            }
        }

        com /= num_boids;
        COM.transform.position = com;
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
