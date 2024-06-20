using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float moveSmoothness = 5f;
    [SerializeField] Transform cameraTransform;


    void Update()
    {
        cameraTransform.transform.position = Vector3.Lerp(cameraTransform.transform.position, transform.position, moveSmoothness * Time.deltaTime);
    }




}