using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level2HUD : MonoBehaviour
{

    [SerializeField] TMP_Text counterText;
    [SerializeField] TMP_Text timeLeftText;

    void Start()
    {
        counterText.text = "0";

    }

    private void OnTransactionCompleted()
    {
        counterText.text = Level2GameplayLogic.score.ToString();
    }


    void Update()
    {
        UpdateTimeLeft();
    }

    void UpdateTimeLeft()
    {
        Level2GameplayLogic.UpdateTimeLeft();

        TimeSpan timeSpan = TimeSpan.FromSeconds(Level2GameplayLogic.timeLeft);
        string timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        timeLeftText.text = timeText;
    }






    void OnEnable()
    {
        Level2GameplayLogic.OnTransactionCompleted += OnTransactionCompleted;
    }

    void OnDisable()
    {
        Level2GameplayLogic.OnTransactionCompleted -= OnTransactionCompleted;

    }
}
