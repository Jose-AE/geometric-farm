using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TractorController))]
public class TractorEffects : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool useEngineSound = true;
    [SerializeField] bool useTrailParticles = true;
    [SerializeField] bool useWheelSpin = true;
    [SerializeField] bool useChasisBob = true;



    [Header("Engine Sound Config")]
    [SerializeField] AudioClip engineSFX;
    [SerializeField] float volume = 0.1f;
    [SerializeField] float pitchMultiplier = 0.1f;
    [SerializeField] float minPitch = 0.5f;
    [SerializeField] float maxPitch = 2.0f;


    [Header("Wheel Config")]
    [SerializeField] Transform wheelBL;
    [SerializeField] Transform wheelBR;
    [SerializeField] Transform wheelFL;
    [SerializeField] Transform wheelFR;


    [Header("Trail Particles Config")]

    [SerializeField] Transform trailsParent;

    [Header("ChasisBob Config")]
    [SerializeField] Transform chasisTransform;
    [SerializeField] float bobIntensity = 0.001f;
    [SerializeField] float bobSpeed = 20f;



    //Private attributes
    private TractorController tractorController;
    private AudioSource engineSoundSource;
    private Vector2 lastMoveDirection = Vector2.zero;
    private float bobTime;

    private PlayerInputActions inputActions;


    void Awake()
    {
        inputActions = new();
        tractorController = GetComponent<TractorController>();
    }


    void Update()
    {
        HandleSpinWheels();
        HandleEngineSound();
        HandleTrailParticles();
        HandleChasisBob();
    }

    private void HandleChasisBob()
    {
        if (!useChasisBob) return;


        bobTime += Time.deltaTime * bobSpeed;

        float sinAmountY = bobIntensity * Mathf.Sin(bobTime);
        chasisTransform.Translate(0, sinAmountY, 0);
    }

    private void HandleTrailParticles()
    {
        foreach (Transform trail in trailsParent)
            trail.gameObject.SetActive(useTrailParticles);
    }

    private void HandleEngineSound()
    {
        if (engineSoundSource == null)
        {
            engineSoundSource = AudioManager.PlaySFX(engineSFX, volume: volume, loop: true);
            engineSoundSource.transform.SetParent(gameObject.transform);
        }

        engineSoundSource.gameObject.SetActive(useEngineSound);

        float velocityMagnitude = tractorController.sphereRb.velocity.magnitude;
        float pitch = Mathf.Clamp(minPitch + velocityMagnitude * pitchMultiplier, minPitch, maxPitch);
        engineSoundSource.pitch = pitch;
    }



    void HandleSpinWheels()
    {
        if (!useWheelSpin) return;

        Vector2 moveInput = inputActions.Level1.Movement.ReadValue<Vector2>();

        if (moveInput.magnitude > 0)
            lastMoveDirection = moveInput;



        float rotationAmount = tractorController.sphereRb.velocity.magnitude * (moveInput.y < 0 ? tractorController.reverseSpeed : tractorController.speed) * 0.5f * Time.deltaTime * Mathf.Sign(lastMoveDirection.y);

        wheelBR.Rotate(-wheelBR.right, rotationAmount, Space.World);
        wheelFR.Rotate(-wheelFR.right, rotationAmount, Space.World);
        wheelBL.Rotate(wheelBL.right, rotationAmount, Space.World);
        wheelFL.Rotate(wheelFL.right, rotationAmount, Space.World);
    }


    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }



}







