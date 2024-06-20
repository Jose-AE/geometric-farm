using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public static class SceneManager
{
    public static void LoadScene(string name)
    {
        Level1GameplayLogic.InitVariables();
        Level2GameplayLogic.InitVariables();
        Level3GameplayLogic.InitVariables();


        UnitySceneManager.LoadScene(name);
    }


}