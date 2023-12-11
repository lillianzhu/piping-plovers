using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    // public int NumberOfFood; //{ get; private set; }

    // private int nutrientAccumulator = 0;

    public UnityEvent<PlayerScore> OnFoodCollected;

    public GameObject bird;

    private TempBirdManager birdManager;

    private TempBirdController controller;

    void OnEnable()
    {
        birdManager = GetComponentInParent<TempBirdManager>();
    }

    public void FoodCollected(int nutrition) {
        // NumberOfFood += nutrition;
        birdManager.birdController.food_points += nutrition;
        if (birdManager.birdController.food_points >= 10) {
            birdManager.birdController.food_points %= 10;
            SpawnNewBird();
        }
        OnFoodCollected.Invoke(this);
    }

    private void SpawnNewBird() {
        // Instantiate(bird, new Vector3(0f, 0f, 0f), Quaternion.identity);
        // Instantiate (copy, position, rotation, parent)
        // GameObject newBird = Instantiate(bird, birdManager.COM.transform.position,
        //     Quaternion.identity, birdManager.birdRoot.transform);
        
        GameObject child = Instantiate(bird, birdManager.birdRoot.transform);
        birdManager.birdController.num_boids += 1;
        child.SetActive(true);
    }

}
