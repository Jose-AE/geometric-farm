using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] TMP_Dropdown graphicsDropdown;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;


    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new();
    }


    public void OnEnableMusic(bool value)
    {
        SettingsManager.useMusic = value;

        AudioManager.SetMusicGroupVolume(value ? 0f : -80f);
    }

    public void OnEnableSFX(bool value)
    {
        SettingsManager.useSFX = value;
        AudioManager.SetSfxGroupVolume(value ? 0f : -80f);
    }

    public void OnChangeQuality(int value)
    {
        SettingsManager.graphicsTier = value;
        QualitySettings.SetQualityLevel(value);
    }


    private void OnOpenSettings(InputAction.CallbackContext context)
    {
        graphicsDropdown.value = SettingsManager.graphicsTier;
        sfxToggle.isOn = SettingsManager.useSFX;
        musicToggle.isOn = SettingsManager.useMusic;


        settingsMenu.SetActive(!settingsMenu.activeInHierarchy);
    }



    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Menu.OpenSettings.performed += OnOpenSettings;
    }


    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Menu.OpenSettings.performed += OnOpenSettings;

    }

}
