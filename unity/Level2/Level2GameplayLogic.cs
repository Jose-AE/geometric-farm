using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;





public enum Item
{
    APPLE,
    TOMATO,
    CORN,
    PUMPKIN,
    CARROT,
    ORANGE
}

public class Transaction
{
    public int payedWith;
    public int totalCost;
    public int expectedChange;
    public string operationType;
    public List<Item> items;
}

public static class Level2GameplayLogic
{
    //Settings
    private const float LEVEL_TIME = 300f;
    private const int MAX_ITEM_PRICE = 10;
    private const int MIN_ITEM_PRICE = 2;
    private const int MAX_CHANGE = 10;


    //Events
    public static Action<Transaction> OnGenerateTransaction;
    public static Action OnTimeRanOut;
    public static Action OnNpcPay;
    public static Action OnTransactionCompleted;


    //Variables
    public static Dictionary<Item, int> itemPrices { private set; get; }

    public static Transaction currentTransaction { private set; get; }

    public static bool gameInProgress { private set; get; } = false;

    public static int score { private set; get; }

    public static float timeLeft { private set; get; }



    public static void InitVariables()
    {
        gameInProgress = false;
    }


    private static void GenerateItemPrices()
    {
        if (!gameInProgress) return;

        itemPrices = new Dictionary<Item, int>();
        HashSet<int> usedPrices = new HashSet<int>();

        for (int i = 0; i < Enum.GetNames(typeof(Item)).Length; i++)
        {
            int price;
            do
            {
                price = UnityEngine.Random.Range(MIN_ITEM_PRICE, MAX_ITEM_PRICE + 1);
            } while (usedPrices.Contains(price));

            itemPrices.Add((Item)i, price);
            usedPrices.Add(price);
        }
    }


    private static void GenerateTransaction()
    {
        if (!gameInProgress) return;

        GenerateItemPrices();

        int randomOperation = UnityEngine.Random.Range(0, 2);

        if (randomOperation == 0)
        {

            int itemsToOrder = UnityEngine.Random.Range(2, 4);
            Hashtable usedItems = new Hashtable();

            List<Item> items = new();

            int totalCost = 0;
            while (itemsToOrder > 0)
            {
                Item randomItem = (Item)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Item)).Length);
                if (usedItems.Contains(randomItem)) continue;

                usedItems.Add(randomItem, true);
                items.Add(randomItem);
                totalCost += itemPrices[randomItem];
                itemsToOrder--;
            }

            int payedWith = totalCost + UnityEngine.Random.Range(1, MAX_CHANGE);

            currentTransaction = new Transaction()
            {
                items = items,
                expectedChange = payedWith - totalCost,
                totalCost = totalCost,
                operationType = "+",
                payedWith = payedWith,
            };

        }
        else
        {
            List<Item> items = new();

            Item randomItem = (Item)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Item)).Length);
            int randomAmmount = UnityEngine.Random.Range(2, 11);
            for (int i = 0; i < randomAmmount; i++)
            {
                items.Add(randomItem);
            }

            int totalCost = itemPrices[randomItem] * randomAmmount;
            int payedWith = totalCost + UnityEngine.Random.Range(1, MAX_CHANGE);

            currentTransaction = new Transaction()
            {
                items = items,
                expectedChange = payedWith - totalCost,
                totalCost = totalCost,
                payedWith = payedWith,
                operationType = "*"
            };

        }

        OnGenerateTransaction?.Invoke(currentTransaction);



    }

    public static void StartGame()
    {
        gameInProgress = true;
        score = 0;
        timeLeft = LEVEL_TIME;

        GenerateTransaction();

    }


    public static bool CheckIfTotalCorrect(int total)
    {
        if (!gameInProgress) return false;


        bool isCorrect = total == currentTransaction.totalCost;

        APIManager.CreateLevel2Stat(isCorrect, currentTransaction.operationType);

        if (isCorrect) OnNpcPay?.Invoke();

        return isCorrect;
    }


    public static bool CheckIfChangeCorrect(int change)
    {
        if (!gameInProgress) return false;

        bool isCorrect = change == currentTransaction.expectedChange;

        APIManager.CreateLevel2Stat(isCorrect, "-");

        if (isCorrect)
        {
            score++;
            OnTransactionCompleted?.Invoke();
            GenerateTransaction();
        }

        return isCorrect;
    }



    public static void UpdateTimeLeft()
    {
        if (!gameInProgress) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            APIManager.CreateLevelScore(score, 2);
            gameInProgress = false;
            OnTimeRanOut?.Invoke();
        }
    }
}
