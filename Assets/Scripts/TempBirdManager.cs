using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBirdManager : MonoBehaviour
{
    [Header("Info")]
    public TempBirdController birdController;

    void Awake()
    { 
        birdController = FindObjectOfType<TempBirdController>();
    }
}
