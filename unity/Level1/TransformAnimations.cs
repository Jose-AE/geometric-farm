using System;
using System.Collections;
using UnityEngine;

public class TransformAnimations : MonoBehaviour
{
    public static IEnumerator Scale(Transform objTransform, Vector3 targetScale, float animationTime, Action onFinish = null)
    {
        Vector3 initialScale = objTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < animationTime)
        {
            objTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objTransform.localScale = targetScale;

        if (onFinish != null)
        {
            onFinish();
        }

    }
}