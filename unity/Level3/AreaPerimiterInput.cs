using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem;



public class AreaPerimiterInput : MonoBehaviour
{
    enum CalculationStage
    {
        ASKING_FOR_PERIMITER,
        ASKING_FOR_AREA,
        WAITING_FOR_NEXT,
    }


    [SerializeField] TMP_Text areaTextInputText;
    [SerializeField] TMP_Text perimiterTextInputText;
    [SerializeField] GameObject waitForNextText;

    [SerializeField] AudioClip correctSfx;
    [SerializeField] AudioClip incorrectSfx;
    [SerializeField] int maxInputLenght = 4;


    private CalculationStage stage;
    private PlayerInputActions inputActions;
    private string input;
    private string lastSubmittedInput;


    void Awake()
    {
        inputActions = new();
        input = "";
    }


    // Start is called before the first frame update
    void Start()
    {
        AskForPerimiter();
    }


    private void AskForPerimiter()
    {
        stage = CalculationStage.ASKING_FOR_PERIMITER;

        waitForNextText.SetActive(false);

        perimiterTextInputText.transform.parent.gameObject.SetActive(true);
        areaTextInputText.transform.parent.gameObject.SetActive(false);

        perimiterTextInputText.text = "";
    }

    private void WaitForNext()
    {
        stage = CalculationStage.WAITING_FOR_NEXT;

        perimiterTextInputText.transform.parent.gameObject.SetActive(false);
        areaTextInputText.transform.parent.gameObject.SetActive(false);

        waitForNextText.SetActive(true);

    }



    private void AskForArea()
    {
        stage = CalculationStage.ASKING_FOR_AREA;
        waitForNextText.SetActive(false);

        areaTextInputText.transform.parent.gameObject.SetActive(true);
        perimiterTextInputText.transform.parent.gameObject.SetActive(false);
        areaTextInputText.text = "";
    }


    public void OnInput(string value)
    {
        if (!Level3GameplayLogic.gameInProgress) return;


        if (stage == CalculationStage.WAITING_FOR_NEXT && value == "submit")
        {
            Level3GameplayLogic.CompleteShape();
            AskForPerimiter();
        }


        if (value == "submit")
        {
            bool isCorrect = false;

            if (lastSubmittedInput != input && input.Length > 0)
            {
                lastSubmittedInput = input;

                try
                {
                    isCorrect = stage == CalculationStage.ASKING_FOR_AREA ? Level3GameplayLogic.CheckIfAreaCorrect(float.Parse(input)) : Level3GameplayLogic.CheckIfPerimiterCorrect(float.Parse(input));
                }
                catch (Exception)
                { isCorrect = false; }

                if (isCorrect)
                {
                    input = "";
                    lastSubmittedInput = "";

                    if (stage == CalculationStage.ASKING_FOR_AREA)
                        WaitForNext();
                    else if (stage == CalculationStage.ASKING_FOR_PERIMITER)
                        AskForArea();
                    else if (stage == CalculationStage.WAITING_FOR_NEXT)
                        AskForPerimiter();
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

        if (stage == CalculationStage.ASKING_FOR_AREA)
            areaTextInputText.text = input;
        else
            perimiterTextInputText.text = input;

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
        inputActions.Level2.Dot.performed += OnInputDot;


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
        inputActions.Level2.Dot.performed -= OnInputDot;


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
    void OnInputDot(InputAction.CallbackContext c) => OnInput(".");

    void OnSubmit(InputAction.CallbackContext c) => OnInput("submit");
    void OnDelete(InputAction.CallbackContext c) => OnInput("cancel");
    #endregion


}