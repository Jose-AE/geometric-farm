using UnityEngine;

public class Level3Debugger : MonoBehaviour
{
    [SerializeField] AreaPerimiterInput areaPerimiterInput;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Debug.isDebugBuild)
        {
            Debug.developerConsoleVisible = true;
            Debug.LogError($"Shape: {Level3GameplayLogic.currentShapeCalculation.shapeType.ToString()} Perimeter: {Level3GameplayLogic.currentShapeCalculation.expectedPerimeter} Area: {Level3GameplayLogic.currentShapeCalculation.expectedArea}");
        }
    }

}