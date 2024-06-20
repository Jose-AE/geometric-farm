using UnityEngine;


public class GoToLevelSelect : MonoBehaviour
{


    public void OnGoToLevelSelect()
    {
        SettingsManager.defaultMainMenuWindow = "LevelSelectMenu";
        SceneManager.LoadScene("MainMenu");
    }



}