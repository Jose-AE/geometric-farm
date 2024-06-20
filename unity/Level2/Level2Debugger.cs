using UnityEngine;

public class Level2Debugger : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Debug.isDebugBuild)
        {
            Debug.developerConsoleVisible = true;
            Debug.LogError($"[Total]: {Level2GameplayLogic.currentTransaction.totalCost} [Change]: {Level2GameplayLogic.currentTransaction.expectedChange}");
        }
    }

}