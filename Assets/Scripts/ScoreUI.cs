using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    // Start is called before the first frame update

    public GameObject root;

    public TempBirdManager birdManager;

    public TempBirdController birdController;
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        birdManager = root.GetComponent<TempBirdManager>();
        birdController = birdManager.birdController;
        Debug.Log(birdController.food_points);
    }

    public void UpdateScoreText(PlayerScore playerScore)
    {
        scoreText.text = "Food: " + birdController.food_points.ToString() + "/10" +
            "\n" + "Flock Size: " + birdController.num_boids.ToString();
    }
}
