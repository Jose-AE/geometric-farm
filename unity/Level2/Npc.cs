using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Npc : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] GameObject transactionUI;

    [SerializeField] Transform skinsParent;
    [SerializeField] ItemIcons icons;


    [HideInInspector] public Transaction transaction;
    [HideInInspector] public bool isTransactionComplete;


    private NavMeshAgent agent;
    private Animator animator;
    private Transform destinationTransform;

    private bool hasDisplayedTransaction = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        transactionUI.SetActive(false);


    }

    void Start()
    {
        GenerateUI();

        SelectRandomSkin();
    }


    private void SelectRandomSkin()
    {
        foreach (Transform skin in skinsParent)
        {
            skin.gameObject.SetActive(false);
        }

        skinsParent.GetChild(Random.Range(0, skinsParent.childCount)).gameObject.SetActive(true);

    }


    private void GenerateUI()
    {
        Dictionary<Item, int> itemCount = new Dictionary<Item, int>();

        foreach (Item item in transaction.items)
        {
            if (!itemCount.ContainsKey(item))
                itemCount[item] = 1;
            else
                itemCount[item] += 1;
        }


        GameObject template = itemList.transform.GetChild(0).gameObject;
        template.SetActive(false);

        foreach (var item in itemCount)
        {
            GameObject itemUI = Instantiate(template);
            itemUI.transform.SetParent(itemList.transform, false);
            itemUI.GetComponentInChildren<TMP_Text>().text = item.Value.ToString();
            itemUI.SetActive(true);


            foreach (var icon in icons.objects)
            {
                if (icon.item == item.Key)
                    itemUI.GetComponentInChildren<Image>().sprite = icon.image;
            }

            //Debug.Log($"Key: {item.Key}, Value: {item.Value}");
        }

        HideUi();

    }


    public void DisplayTransaction()
    {
        if (hasDisplayedTransaction || transaction == null) return;

        hasDisplayedTransaction = true;
        animator.SetTrigger("Talk");

        Invoke("ShowUi", 1.5f);
    }

    private void ShowUi()
    {
        transactionUI.SetActive(true);
        StartCoroutine(TransformAnimations.Scale(transactionUI.transform, Vector3.one, 0.5f));
        transactionUI.transform.SetParent(null);
    }

    public void HideUi()
    {
        StartCoroutine(TransformAnimations.Scale(transactionUI.transform, Vector3.zero, 0.5f, () =>
        {
            transactionUI.SetActive(false);
        }));
    }

    public void MoveTo(Transform tra)
    {
        agent.SetDestination(tra.position);
        destinationTransform = tra;
    }



    void Update()
    {
        animator.SetBool("Moving", agent.velocity.magnitude > 0.1);



        if (destinationTransform && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(transform.position);
            agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, destinationTransform.rotation, Time.deltaTime * 2f);
            DisplayTransaction();
        }
    }
}