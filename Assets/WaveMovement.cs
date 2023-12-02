using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    public float speed = 2f;
    public float height = 0.05f;
    public float startY = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    void Update()
    {
        var pos = transform.position;
        var newY = startY + height * Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
