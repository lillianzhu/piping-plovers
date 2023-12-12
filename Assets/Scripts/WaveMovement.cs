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
      // Add variation in individual wave movement
      // But this only happens at initialization
      speed = Random.Range(0.3f, 0.9f);
      randSpeedMult = Random.Range(5, 18);
      startZ = transform.localPosition.z;
    }



    void Update()
    {
      var pos = transform.localPosition;
      var newZ = startZ + height * randSpeedMult * Mathf.Sin(Time.time * speed);
      transform.position = new Vector3(pos.x, pos.y, newZ);
    }
}
