
using TMPro;
using UnityEngine;

public class CashSpawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform coinsParent;
    [SerializeField] TMP_Text totalText;

    [SerializeField] AudioClip coinSfx;


    void Start()
    {
        totalText.text = "";
    }



    private void OnNpcPay()
    {
        Transaction trans = Level2GameplayLogic.currentTransaction;
        AudioManager.PlaySFX(coinSfx);

        for (int i = 0; i < trans.payedWith; i++)
        {
            float offset = 0.3f;
            Vector3 pos = spawnPoint.position + new Vector3(Random.Range(0, offset), Random.Range(0, offset), Random.Range(0, offset));
            GameObject c = Instantiate(coinPrefab, pos, Quaternion.identity);
            c.transform.SetParent(coinsParent, true);
        }

        totalText.text = "$" + trans.payedWith.ToString();
    }

    private void OnTransactionCompleted()
    {
        totalText.text = "";

        foreach (Transform coin in coinsParent)
        {
            StartCoroutine(TransformAnimations.Scale(coin, Vector3.zero, 1f));
        }
    }



    void OnEnable()
    {
        Level2GameplayLogic.OnNpcPay += OnNpcPay;
        Level2GameplayLogic.OnTransactionCompleted += OnTransactionCompleted;

    }



    void OnDisable()
    {
        Level2GameplayLogic.OnNpcPay -= OnNpcPay;
        Level2GameplayLogic.OnTransactionCompleted -= OnTransactionCompleted;

    }


}
