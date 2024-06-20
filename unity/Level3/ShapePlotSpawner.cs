using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ShapePlotSpawner : MonoBehaviour
{
    [System.Serializable]
    class ShapeData
    {
        public GameObject prefab;
        public Shape shape;

    }


    [SerializeField] Transform spawnPos;
    [SerializeField] ShapeData[] shapes;
    [SerializeField] float animationTime = 0.1f;
    [SerializeField] string lengthPrefix = "M";


    private GameObject currentShapePlot;
    private GameObject currentShapePlotFences;
    private GameObject currentShapePlotAnimals;


    void Awake()
    {
        currentShapePlot = new GameObject();
    }


    private void AddLabels(ShapeCalculation shapecalculation)
    {
        TMP_Text[] labelsText = currentShapePlot.transform.GetChild(2).GetComponentsInChildren<TMP_Text>();

        switch (shapecalculation.shapeType)
        {
            case Shape.Circulo:
                labelsText[0].text = shapecalculation.lengths["radius"].ToString() + lengthPrefix;
                break;
            case Shape.Cuadrado:
                labelsText[0].text = shapecalculation.lengths["side"].ToString() + lengthPrefix;
                break;
            case Shape.Hexagono:
                labelsText[0].text = shapecalculation.lengths["side"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["apothem"].ToString() + lengthPrefix;
                break;
            case Shape.Octagono:
                labelsText[0].text = shapecalculation.lengths["side"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["apothem"].ToString() + lengthPrefix;
                break;
            case Shape.Paralelogramo:
                labelsText[0].text = shapecalculation.lengths["base"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["height"].ToString() + lengthPrefix;
                labelsText[2].text = shapecalculation.lengths["side"].ToString() + lengthPrefix;
                break;
            case Shape.Pentagono:
                labelsText[0].text = shapecalculation.lengths["side"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["apothem"].ToString() + lengthPrefix;
                break;
            case Shape.Rectangulo:
                labelsText[0].text = shapecalculation.lengths["length"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["width"].ToString() + lengthPrefix;
                break;
            case Shape.Rombo:
                labelsText[0].text = shapecalculation.lengths["side"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["diagonal1"].ToString() + lengthPrefix;
                labelsText[2].text = shapecalculation.lengths["diagonal2"].ToString() + lengthPrefix;
                break;
            case Shape.Trapecio:
                labelsText[0].text = shapecalculation.lengths["longBase"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["shortBase"].ToString() + lengthPrefix;
                labelsText[2].text = shapecalculation.lengths["height"].ToString() + lengthPrefix;
                labelsText[3].text = shapecalculation.lengths["sides"].ToString() + lengthPrefix;
                break;
            case Shape.Triangulo:
                labelsText[0].text = shapecalculation.lengths["base"].ToString() + lengthPrefix;
                labelsText[1].text = shapecalculation.lengths["height"].ToString() + lengthPrefix;
                break;
            default:
                break;
        }
    }


    private void SpawnPlot(ShapeCalculation shape)
    {
        StartCoroutine(TransformAnimations.Scale(currentShapePlot.transform, Vector3.zero, animationTime, () =>
               {
                   Destroy(currentShapePlot);

                   foreach (ShapeData data in shapes)
                   {
                       if (data.shape == shape.shapeType)
                       {
                           currentShapePlot = Instantiate(data.prefab);
                           AddLabels(shape);
                           currentShapePlot.transform.position = spawnPos.position;

                           currentShapePlotFences = currentShapePlot.transform.GetChild(0).gameObject;
                           currentShapePlotAnimals = currentShapePlot.transform.GetChild(1).gameObject;
                           currentShapePlotFences.gameObject.SetActive(false);
                           currentShapePlotAnimals.gameObject.SetActive(false);



                           currentShapePlot.transform.localScale = Vector3.zero;
                           StartCoroutine(TransformAnimations.Scale(currentShapePlot.transform, Vector3.one, animationTime));
                       }
                   }

               }));
    }


    private void HideLabels()
    {
        currentShapePlot.transform.GetChild(2).gameObject.SetActive(false);
    }

    private void SpawnFences()
    {
        currentShapePlotFences.SetActive(true);
        currentShapePlotFences.transform.localScale = new Vector3(1, 1, 0);
        StartCoroutine(TransformAnimations.Scale(currentShapePlotFences.transform, Vector3.one, 0.5f));
    }

    private void SpawnAnimals()
    {
        HideLabels();
        currentShapePlotAnimals.SetActive(true);


        //hide all animal types
        foreach (Transform animalCol in currentShapePlotAnimals.transform)
        {
            animalCol.gameObject.SetActive(false);
        }


        int randomAnimalType = Random.Range(0, currentShapePlotAnimals.transform.childCount);

        currentShapePlotAnimals.transform.GetChild(randomAnimalType).gameObject.SetActive(true);

        foreach (Transform animal in currentShapePlotAnimals.transform.GetChild(randomAnimalType))
        {
            animal.Rotate(animal.up * Random.Range(0, 360));
        }


        currentShapePlotAnimals.transform.localScale = Vector3.zero;
        StartCoroutine(TransformAnimations.Scale(currentShapePlotAnimals.transform, Vector3.one, animationTime));
    }





    void OnEnable()
    {
        Level3GameplayLogic.OnGenerateShapeCalculation += SpawnPlot;
        Level3GameplayLogic.OnPerimiterCompleted += SpawnFences;
        Level3GameplayLogic.OnAreaCompleted += SpawnAnimals;

    }

    void OnDisable()
    {
        Level3GameplayLogic.OnGenerateShapeCalculation -= SpawnPlot;
        Level3GameplayLogic.OnPerimiterCompleted -= SpawnFences;
        Level3GameplayLogic.OnAreaCompleted -= SpawnAnimals;


    }

}
