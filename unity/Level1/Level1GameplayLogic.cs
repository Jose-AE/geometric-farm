using UnityEngine;
using System;


public enum Shape
{
    Circulo,
    Triangulo,
    Rectangulo,
    Paralelogramo,
    Cuadrado,
    Rombo,
    Trapecio,
    Pentagono,
    Hexagono,
    Octagono
}

public static class Level1GameplayLogic
{

    //Settings 
    private const float LEVEL_TIME = 300f;
    private const int CORRECT_SHAPE_SCORE = 100;
    private const int INCORRECT_SHAPE_SCORE = -50;


    //Events
    public static Action<Shape> OnCollectCorrectShape;
    public static Action<Shape> OnCollectIncorrectShape;

    public static Action<Shape> OnGenerateShape;
    public static Action OnTimeRanOut;



    //Variables
    public static bool gameInProgress { private set; get; }

    public static int score { private set; get; }

    public static Shape currentShape { private set; get; }

    public static float timeLeft { private set; get; }


    public static void InitVariables()
    {
        gameInProgress = false;
    }

    private static void GenerateShape()
    {
        if (!gameInProgress) return;

        currentShape = (Shape)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Shape)).Length);
        OnGenerateShape?.Invoke(currentShape);
    }

    public static void StartGame()
    {
        gameInProgress = true;
        score = 0;
        timeLeft = LEVEL_TIME;
        GenerateShape();
    }

    private static void SetScore(int s)
    {
        if (!gameInProgress) return;

        score = Math.Max(0, s);
    }

    public static void CollectShape(Shape shape)
    {
        if (!gameInProgress) return;

        APIManager.CreateLevel1Stat(shape == currentShape, currentShape.ToString(), shape.ToString());


        if (shape == currentShape)
        {
            GenerateShape();
            SetScore(score + CORRECT_SHAPE_SCORE);
            OnCollectCorrectShape?.Invoke(shape);

        }
        else
        {
            SetScore(score + INCORRECT_SHAPE_SCORE);
            OnCollectIncorrectShape?.Invoke(shape);
        }

    }

    public static void UpdateTimeLeft()
    {
        if (!gameInProgress) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            APIManager.CreateLevelScore(score, 1);
            gameInProgress = false;
            OnTimeRanOut?.Invoke();
        }

    }

}