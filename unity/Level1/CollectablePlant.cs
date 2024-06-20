using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectablePlant : MonoBehaviour
{

    [System.Serializable]
    public struct PlantData
    {
        public Shape shapeType;
        public Sprite image;
    }

    public PlantData plantData;

    private float collectTime = 1f;

    public void Collect()
    {
        Level1GameplayLogic.CollectShape(plantData.shapeType);

        StartCoroutine(TransformAnimations.Scale(transform, Vector3.zero, collectTime));
        Destroy(gameObject, collectTime + 1f);
    }





}
