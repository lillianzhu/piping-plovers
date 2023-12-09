using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerScore playerScore = other.GetComponent<PlayerScore>();

        if (playerScore != null)
        {
            playerScore.FoodCollected();
            gameObject.SetActive(false);
        }
    }
}
