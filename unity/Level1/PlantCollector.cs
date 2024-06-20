
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class PlantCollector : MonoBehaviour
{

    [SerializeField] Transform shapePreview;
    [SerializeField] Transform shapePreviewPivot;

    [SerializeField] Image shapePreviewImage;

    [SerializeField] float previewPopTime = 0.3f;
    [SerializeField] AudioClip previewSound;


    private CollectablePlant selectedPlant;
    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new();
    }

    void Start()
    {
        shapePreview.SetParent(null);
        shapePreview.localScale = Vector3.zero;
    }

    void Update()
    {
        shapePreview.position = shapePreviewPivot.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectablePlant"))
        {
            AudioManager.PlaySFX(previewSound);

            StartCoroutine(TransformAnimations.Scale(shapePreview, Vector3.one, previewPopTime));


            selectedPlant = other.GetComponent<CollectablePlant>();
            shapePreviewImage.sprite = selectedPlant.plantData.image;

        }

    }

    void OnTriggerExit(Collider other)
    {
        CleanSelectedShape();
    }

    void CleanSelectedShape()
    {
        selectedPlant = null;
        StartCoroutine(TransformAnimations.Scale(shapePreview, Vector3.zero, previewPopTime));
    }


    private void Collect(InputAction.CallbackContext context)
    {
        if (selectedPlant == null) return;



        selectedPlant.Collect();
        CleanSelectedShape();

    }



    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Level1.Collect.performed += Collect;
    }


    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Level1.Collect.performed -= Collect;
    }

}
