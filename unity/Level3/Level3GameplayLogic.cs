using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ShapeCalculation
{
    public Shape shapeType;
    public float expectedArea;
    public float expectedPerimeter;

    public Dictionary<string, int> lengths;
}


public class Level3GameplayLogic : MonoBehaviour
{

    //Settings
    private const float LEVEL_TIME = 300f;
    private const int MAX_SHAPE_LENGTH = 20;
    private const int MIN_SHAPE_LENGTH = 2;
    private const float PI = 3.14F;




    //Events
    public static Action<ShapeCalculation> OnGenerateShapeCalculation;
    public static Action OnAreaCompleted;
    public static Action OnPerimiterCompleted;
    public static Action OnCompleted;



    public static Action OnTimeRanOut;


    //Variables
    public static ShapeCalculation currentShapeCalculation { private set; get; }

    public static bool gameInProgress { private set; get; } = false;

    public static int score { private set; get; }

    public static float timeLeft { private set; get; }



    public static void InitVariables()
    {
        gameInProgress = false;
    }



    private static void GenerateShapeCalculation()
    {
        if (!gameInProgress) return;


        Shape randomShape = (Shape)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Shape)).Length);
        float expectedArea = 0;
        float expectedPerimeter = 0;
        Dictionary<string, int> lengths = new();

        switch (randomShape)
        {
            case Shape.Circulo:
                lengths.Add("radius", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Radius
                expectedArea = PI * lengths["radius"] * lengths["radius"];
                expectedPerimeter = 2 * lengths["radius"] * PI;
                break;
            case Shape.Cuadrado:
                lengths.Add("side", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Side length
                expectedArea = lengths["side"] * lengths["side"];
                expectedPerimeter = 4 * lengths["side"];
                break;
            case Shape.Hexagono:
                lengths.Add("side", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Side length
                lengths.Add("apothem", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Apothem
                expectedPerimeter = 6 * lengths["side"];
                expectedArea = expectedPerimeter * lengths["apothem"] / 2;
                break;
            case Shape.Octagono:
                lengths.Add("side", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Side 
                lengths.Add("apothem", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Apothem
                expectedPerimeter = 8 * lengths["side"];
                expectedArea = expectedPerimeter * lengths["apothem"] / 2;
                break;
            case Shape.Paralelogramo:
                lengths.Add("base", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Base
                lengths.Add("height", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Height
                lengths.Add("side", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Side length
                expectedArea = lengths["base"] * lengths["height"];
                expectedPerimeter = 2 * (lengths["base"] + lengths["side"]);
                break;
            case Shape.Pentagono:
                lengths.Add("side", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Side 
                lengths.Add("apothem", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Apothem
                expectedPerimeter = 5 * lengths["side"];
                expectedArea = expectedPerimeter * lengths["apothem"] / 2;
                break;
            case Shape.Rectangulo:
                lengths.Add("length", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Length
                lengths.Add("width", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Width
                expectedArea = lengths["length"] * lengths["width"];
                expectedPerimeter = 2 * (lengths["length"] + lengths["width"]);
                break;
            case Shape.Rombo:
                lengths.Add("side", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Side length
                lengths.Add("diagonal1", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Diagonal 1
                lengths.Add("diagonal2", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Diagonal 2
                expectedArea = lengths["diagonal1"] * lengths["diagonal2"] / 2;
                expectedPerimeter = 4 * lengths["side"];
                break;
            case Shape.Trapecio:
                lengths.Add("longBase", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Base 1
                lengths.Add("shortBase", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Base 2
                lengths.Add("height", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Height
                lengths.Add("sides", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//left and right Side 

                expectedArea = (lengths["longBase"] + lengths["shortBase"]) / 2 * lengths["height"];
                expectedPerimeter = lengths["longBase"] + lengths["shortBase"] + 2 * lengths["sides"];
                break;
            case Shape.Triangulo:
                lengths.Add("base", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Base
                lengths.Add("height", UnityEngine.Random.Range(MIN_SHAPE_LENGTH, MAX_SHAPE_LENGTH + 1));//Height
                expectedArea = lengths["base"] * lengths["height"] / 2;
                expectedPerimeter = lengths["base"] * 3;
                break;
            default:
                break;
        }

        expectedArea = Mathf.Round(expectedArea * 10.0f) * 0.1f;
        expectedPerimeter = Mathf.Round(expectedPerimeter * 10.0f) * 0.1f;


        currentShapeCalculation = new ShapeCalculation()
        {
            shapeType = randomShape,
            expectedArea = expectedArea,
            expectedPerimeter = expectedPerimeter,
            lengths = lengths,
        };

        OnGenerateShapeCalculation?.Invoke(currentShapeCalculation);


    }

    public static void StartGame()
    {
        gameInProgress = true;
        score = 0;
        timeLeft = LEVEL_TIME;

        GenerateShapeCalculation();
    }


    public static bool CheckIfPerimiterCorrect(float perimiter)
    {
        if (!gameInProgress) return false;

        perimiter = Mathf.Round(perimiter * 10.0f) * 0.1f;


        bool isCorrect = perimiter == currentShapeCalculation.expectedPerimeter;

        APIManager.CreateLevel3Stat(isCorrect, "area", currentShapeCalculation.shapeType.ToString());

        if (isCorrect)
            OnPerimiterCompleted?.Invoke();

        return isCorrect;

    }


    public static bool CheckIfAreaCorrect(float area)
    {
        if (!gameInProgress) return false;

        area = Mathf.Round(area * 10.0f) * 0.1f;



        bool isCorrect = area == currentShapeCalculation.expectedArea;

        APIManager.CreateLevel3Stat(isCorrect, "perimiter", currentShapeCalculation.shapeType.ToString());

        if (isCorrect)
            OnAreaCompleted?.Invoke();



        return isCorrect;
    }

    public static void CompleteShape()
    {
        if (!gameInProgress) return;

        score++;
        GenerateShapeCalculation();
        OnCompleted?.Invoke();
    }



    public static void UpdateTimeLeft()
    {
        if (!gameInProgress) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            APIManager.CreateLevelScore(score, 3);
            gameInProgress = false;
            OnTimeRanOut?.Invoke();
        }
    }



}
