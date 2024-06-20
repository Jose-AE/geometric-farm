using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostBook : MonoBehaviour
{
    [SerializeField] Transform itemTagsParent;


    private Dictionary<Item, TMP_Text> priceTags = new Dictionary<Item, TMP_Text>();

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Transform item in itemTagsParent)
        {
            priceTags[(Item)Enum.Parse(typeof(Item), item.name)] = item.GetComponentInChildren<TMP_Text>();
        }
    }

    private void OnGenerateTransaction(Transaction transaction)
    {
        foreach (var kvp in Level2GameplayLogic.itemPrices)
        {

            priceTags[kvp.Key].text = "$" + kvp.Value.ToString();
        }
    }


    void OnEnable()
    {
        Level2GameplayLogic.OnGenerateTransaction += OnGenerateTransaction;
    }



    void OnDisable()
    {
        Level2GameplayLogic.OnGenerateTransaction -= OnGenerateTransaction;
    }
}
