using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    [SerializeField] private Button loginButton;

    private string listId;
    private string groupId;
    private int gender;


    void Start()
    {
        gender = 0;
        listId = "";
        groupId = "";
        UpdateLoginButtonStatus();

        MenuManager.OpenMenu(SettingsManager.defaultMainMenuWindow);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Debug.isDebugBuild)
        {
            OnBypassLoginButtonClick();
        }
    }

    public void OnLoginButtonClick()
    {
        SettingsManager.gender = gender == 0 ? "M" : "F";
        SettingsManager.studentListNum = Convert.ToInt32(listId);
        SettingsManager.studentGroup = groupId;

        loginButton.interactable = false;

        APIManager.VerifyStudent(
            SettingsManager.studentListNum,
            SettingsManager.studentGroup,
            () => { MenuManager.OpenMenu("MainMenu"); loginButton.interactable = true; },
            () => { loginButton.interactable = true; }
            );

    }

    public void OnBypassLoginButtonClick()
    {
        MenuManager.OpenMenu("MainMenu");
    }




    public void OnGroupIdInputFieldChanged(string text)
    {
        groupId = text;
        UpdateLoginButtonStatus();
    }
    public void OnListIdInputFieldChanged(string text)
    {
        listId = text;
        UpdateLoginButtonStatus();
    }

    public void OnGenderDropdownChanged(int index)
    {
        gender = index;
    }


    public void UpdateLoginButtonStatus()
    {
        loginButton.interactable = groupId.Length > 0 && listId.Length > 0;
    }
}
