using System;
using TMPro;
using UnityEngine;


public class Level1HUD : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text shapeText;
    [SerializeField] TMP_Text timeLeftText;


    [SerializeField] AudioClip correctShapeSound;
    [SerializeField] AudioClip wrongShapeSound;


    void Start()
    {
        UpdateScore();
    }

    void Update()
    {
        UpdateTimeLeft();
    }

    void UpdateTimeLeft()
    {
        Level1GameplayLogic.UpdateTimeLeft();

        TimeSpan timeSpan = TimeSpan.FromSeconds(Level1GameplayLogic.timeLeft);
        string timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        timeLeftText.text = timeText;
    }


    private void UpdateScore()
    {
        coinText.text = Level1GameplayLogic.score.ToString("n0");
    }

    private void OnCollectIncorrectShape(Shape shape)
    {
        UpdateScore();
        AudioManager.PlaySFX(wrongShapeSound);
    }

    private void OnCollectCorrectShape(Shape shape)
    {
        UpdateScore();
        AudioManager.PlaySFX(correctShapeSound);
    }


    private void OnGenerateShape(Shape shape)
    {
        shapeText.text = shape.ToString();
    }

    void OnEnable()
    {
        Level1GameplayLogic.OnCollectCorrectShape += OnCollectCorrectShape;
        Level1GameplayLogic.OnCollectIncorrectShape += OnCollectIncorrectShape;
        Level1GameplayLogic.OnGenerateShape += OnGenerateShape;
    }



    void OnDisable()
    {
        Level1GameplayLogic.OnCollectCorrectShape -= OnCollectCorrectShape;
        Level1GameplayLogic.OnCollectIncorrectShape -= OnCollectIncorrectShape;
        Level1GameplayLogic.OnGenerateShape -= OnGenerateShape;

    }
}
