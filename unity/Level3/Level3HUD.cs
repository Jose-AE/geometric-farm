using System;
using TMPro;
using UnityEngine;


public class Level3HUD : MonoBehaviour
{
    [SerializeField] TMP_Text counterText;
    [SerializeField] TMP_Text timeLeftText;

    void Start()
    {
        counterText.text = "0";

    }

    private void OnTransactionCompleted()
    {
        counterText.text = Level3GameplayLogic.score.ToString();
    }


    void Update()
    {
        UpdateTimeLeft();
    }

    void UpdateTimeLeft()
    {
        Level3GameplayLogic.UpdateTimeLeft();

        TimeSpan timeSpan = TimeSpan.FromSeconds(Level3GameplayLogic.timeLeft);
        string timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        timeLeftText.text = timeText;
    }






    void OnEnable()
    {
        Level3GameplayLogic.OnCompleted += OnTransactionCompleted;
    }

    void OnDisable()
    {
        Level3GameplayLogic.OnCompleted -= OnTransactionCompleted;

    }
}
