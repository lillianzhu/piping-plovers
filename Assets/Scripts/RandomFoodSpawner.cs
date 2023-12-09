using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFoodSpawner : MonoBehaviour
{
    public GameObject[] myObjects;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            int randomIndex = Random.Range(0, myObjects.Length);
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-18, 80), 0, Random.Range(-10, -15));
            Instantiate(myObjects[randomIndex], randomSpawnPosition, Quaternion.identity);
        }


    }
}
