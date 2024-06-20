using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    public void OnClickLevel(string level)
    {
        SceneManager.LoadScene(level);
    }



    public void OnClickReturn()
    {
        MenuManager.OpenMenu("MainMenu");
    }
}
