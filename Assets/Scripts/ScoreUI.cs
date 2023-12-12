using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    // Start is called before the first frame update

    public GameObject winTextObject;
    public GameObject loseTextObject;
    public GameObject playAgainObject;

    public GameObject root;

    public TempBirdManager birdManager;

    public TempBirdController birdController;

   
    void Start()
    {
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        playAgainObject.SetActive(false);


        scoreText = GetComponent<TextMeshProUGUI>();
        birdManager = root.GetComponent<TempBirdManager>();
        birdController = birdManager.birdController;
        //Debug.Log(birdController.food_points);
    }

    public void UpdateScoreText(PlayerScore playerScore)
    {
        scoreText.text = "Food: " + birdController.food_points.ToString() + "/10" +
            "\n" + "Flock Size: " + birdController.num_boids.ToString() + "/5";

        if (birdController.num_boids >= 2)
        {
            // Display the win text.
            winTextObject.SetActive(true);
            playAgainObject.SetActive(true);
        }
        //if (birdController.num_boids < 1)
        //{
        //    // Display the win text.
        //    loseTextObject.SetActive(true);
        //    playAgainObject.SetActive(true);
        //}
    }

    public void SetGameOverText()
    {
        loseTextObject.SetActive(true);
        playAgainObject.SetActive(true);
    }
}
