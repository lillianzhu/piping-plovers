using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    public float speed = 0.5f;
    public float height = 0.5f;
    public float startZ = 0.0f;
    public float randSpeedMult = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
      randSpeedMult = Random.Range(5, 20);
      startZ = transform.localPosition.z;
    }



    void Update()
    {
        var pos = transform.localPosition;
        var newZ = startZ + height * randSpeedMult * Mathf.Sin(Time.time * speed);
        Debug.Log(randSpeedMult);
        transform.position = new Vector3(pos.x, pos.y, newZ);
    }
}
