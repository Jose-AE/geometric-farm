using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem;


public class CashRegister : MonoBehaviour
{


    [SerializeField] TMP_Text changeTextInputText;
    [SerializeField] TMP_Text totalTextInputText;
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip correctSfx;
    [SerializeField] AudioClip incorrectSfx;
    [SerializeField] int maxInputLenght = 6;



    private PlayerInputActions inputActions;
    private string input;
    private string lastSubmittedInput;
    private bool calculatingTotal;


    void Awake()
    {
        inputActions = new();
        input = "";
    }


    // Start is called before the first frame update
    void Start()
    {
        UseTotalMode();
    }


    private void UseChangeMode()
    {
        calculatingTotal = false;

        changeTextInputText.transform.parent.gameObject.SetActive(true);
        changeTextInputText.text = "";
    }


    private void UseTotalMode()
    {
        calculatingTotal = true;

        totalTextInputText.transform.parent.gameObject.SetActive(true);
        changeTextInputText.transform.parent.gameObject.SetActive(false);
        totalTextInputText.text = "";
    }


    public void OnInput(string value)
    {
        if (!Level2GameplayLogic.gameInProgress) return;


        AudioManager.PlaySFX(clickSound);

        if (value == "submit")
        {
            bool isCorrect = false;

            if (lastSubmittedInput != input && input.Length > 0)
            {
                lastSubmittedInput = input;

                isCorrect = calculatingTotal ? Level2GameplayLogic.CheckIfTotalCorrect(Convert.ToInt32(input)) : Level2GameplayLogic.CheckIfChangeCorrect(Convert.ToInt32(input));
                if (isCorrect)
                {
                    input = "";
                    lastSubmittedInput = "";

                    if (calculatingTotal)
                        UseChangeMode();
                    else
                        UseTotalMode();
                }

            }
            if (isCorrect)
                AudioManager.PlaySFX(correctSfx);
            else
                AudioManager.PlaySFX(incorrectSfx);

        }
        else if (value == "cancel")
        {
            if (input.Length == 0) return;

            input = input.Substring(0, input.Length - 1);
        }
        else
        {
            if (input.Length < maxInputLenght)
                input += value;
        }

        if (calculatingTotal)
            totalTextInputText.text = input;
        else
            changeTextInputText.text = input;

    }


    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Level2.Num0.performed += OnInput0;
        inputActions.Level2.Num1.performed += OnInput1;
        inputActions.Level2.Num2.performed += OnInput2;
        inputActions.Level2.Num3.performed += OnInput3;
        inputActions.Level2.Num4.performed += OnInput4;
        inputActions.Level2.Num5.performed += OnInput5;
        inputActions.Level2.Num6.performed += OnInput6;
        inputActions.Level2.Num7.performed += OnInput7;
        inputActions.Level2.Num8.performed += OnInput8;
        inputActions.Level2.Num9.performed += OnInput9;

        inputActions.Level2.Submit.performed += OnSubmit;
        inputActions.Level2.Delete.performed += OnDelete;
    }


    void OnDisable()
    {
        inputActions.Disable();

        inputActions.Level2.Num0.performed -= OnInput0;
        inputActions.Level2.Num1.performed -= OnInput1;
        inputActions.Level2.Num2.performed -= OnInput2;
        inputActions.Level2.Num3.performed -= OnInput3;
        inputActions.Level2.Num4.performed -= OnInput4;
        inputActions.Level2.Num5.performed -= OnInput5;
        inputActions.Level2.Num6.performed -= OnInput6;
        inputActions.Level2.Num7.performed -= OnInput7;
        inputActions.Level2.Num8.performed -= OnInput8;
        inputActions.Level2.Num9.performed -= OnInput9;

        inputActions.Level2.Submit.performed -= OnSubmit;
        inputActions.Level2.Delete.performed -= OnDelete;
    }



    #region INPUT_FUNCTIONS
    void OnInput0(InputAction.CallbackContext c) => OnInput("0");
    void OnInput1(InputAction.CallbackContext c) => OnInput("1");
    void OnInput2(InputAction.CallbackContext c) => OnInput("2");
    void OnInput3(InputAction.CallbackContext c) => OnInput("3");
    void OnInput4(InputAction.CallbackContext c) => OnInput("4");
    void OnInput5(InputAction.CallbackContext c) => OnInput("5");
    void OnInput6(InputAction.CallbackContext c) => OnInput("6");
    void OnInput7(InputAction.CallbackContext c) => OnInput("7");
    void OnInput8(InputAction.CallbackContext c) => OnInput("8");
    void OnInput9(InputAction.CallbackContext c) => OnInput("9");
    void OnSubmit(InputAction.CallbackContext c) => OnInput("submit");
    void OnDelete(InputAction.CallbackContext c) => OnInput("cancel");
    #endregion
}
