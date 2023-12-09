using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    public int NumberOfFood { get; private set; }

    public UnityEvent<PlayerScore> OnFoodCollected;

    public void FoodCollected() {
        NumberOfFood++;
        OnFoodCollected.Invoke(this);
    }

}
