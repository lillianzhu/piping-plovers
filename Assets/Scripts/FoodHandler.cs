using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHandler : MonoBehaviour
{

    public int nutrition = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Make spawn disappear if wave or a bird hits it
        gameObject.SetActive(false);
        PlayerScore playerScore = other.GetComponent<PlayerScore>();


        if (playerScore != null)
        {
            // If spawn was elminated by a player
            playerScore.FoodCollected(nutrition);
        }
    }
}
