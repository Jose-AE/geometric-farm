using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void OnClickPlayButton()
    {
        MenuManager.OpenMenu("LevelSelectMenu");

    }

    public void OnClickExitButton()
    {
        MenuManager.OpenMenu("LoginMenu");
    }

}
