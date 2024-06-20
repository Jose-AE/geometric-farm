using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegisterButton : MonoBehaviour
{
    [SerializeField] string value;
    [SerializeField] CashRegister cashRegister;


    private Material outlineMaterial;
    private MeshRenderer meshRenderer;
    private Material[] originalMaterials;


    void Awake()
    {
        outlineMaterial = Resources.Load<Material>("Materials/OutlineMaterial");
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterials = meshRenderer.materials;
    }



    void OnMouseDown()
    {

        if (!Level2GameplayLogic.gameInProgress) return;

        cashRegister.OnInput(value);
    }

    void OnMouseEnter()
    {
        meshRenderer.materials = new Material[] { originalMaterials[0], outlineMaterial };
    }

    void OnMouseExit()
    {
        meshRenderer.materials = originalMaterials;

    }



}
