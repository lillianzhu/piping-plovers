using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBirdManager : MonoBehaviour
{
    [Header("Info")]
    public TempBirdController birdController;

    public GameObject birdRoot;
    public GameObject COM;

    void Awake()
    { 
        birdController = FindObjectOfType<TempBirdController>();
        birdController.birdRoot = birdRoot;
        birdController.COM = COM;
    }

}
