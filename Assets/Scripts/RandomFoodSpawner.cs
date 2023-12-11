using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFoodSpawner : MonoBehaviour
{
    public GameObject[] myObjects;

    // Spawn food per second...
    public float spawnRate = 0.1f;
    // Food lifespan before automatically despawning
    public float shrimp_lifespan = 8f;

    public float worm_lifespan = 5f;

    public float clam_lifespan = 20f;

    // Probability of spawning
    // These are independent, do not need to add to 1
    public float shrimp_p = 0.3f;

    public float worm_p = 0.5f;

    public float clam_p = 0.2f;


    // Update is called once per frame
    void Start() {
        InvokeRepeating("SpawnCritter", 1f, spawnRate);
    }
    void SpawnCritter()
    {
        float rand = Random.Range(0f, 1f);
        if (rand < clam_p) {
            // Spawn a clam with 20% chance
            // Remember that foodTypes that spawn close to the waves are more likely to despawn
            Vector3 pos = new Vector3(Random.Range(-18, 80), 0, Random.Range(-14, -19));
            GameObject critter = Instantiate(myObjects[0], pos, Quaternion.identity);
            Destroy(critter, clam_lifespan); // Critter despawns after lifespan seconds
        }
        if (rand < shrimp_p) {
            // Spawn a shrimp with 30% chance
            Vector3 pos = new Vector3(Random.Range(-18, 80), 0, Random.Range(-10, -15));
            GameObject critter = Instantiate(myObjects[1], pos, Quaternion.identity);
            Destroy(critter, shrimp_lifespan); // Critter despawns after lifespan seconds
        }
        if (rand < worm_p) {
            // Spawn a worm with 50% chance
            Vector3 pos = new Vector3(Random.Range(-18, 80), 0, Random.Range(-7, -12));
            GameObject critter = Instantiate(myObjects[2], pos, Quaternion.identity);
            Destroy(critter, worm_lifespan); // Critter despawns after lifespan seconds
        }
        
    }
}
