using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
class Page
{
    [SerializeField] public Sprite image;
    [SerializeField] public string text;

}

public class TutorialUI : MonoBehaviour
{

    [SerializeField] Page[] pages;

    [SerializeField] Button nextButton;
    [SerializeField] Button backButton;

    [SerializeField] GameObject HUD;

    [SerializeField] Transform pagesParent;

    private GameObject pageTemplate;
    private int currentPageIndex = 0;

    void Awake()
    {
        pageTemplate = pagesParent.GetChild(0).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);

        HUD.SetActive(false);
        GeneratePages();
        UpdateButtons();
        ShowPage(0);
    }

    private void GeneratePages()
    {
        foreach (var page in pages)
        {
            pageTemplate.SetActive(false);
            GameObject p = Instantiate(pageTemplate);
            p.transform.SetParent(pageTemplate.transform.parent, false);
            p.GetComponentInChildren<TMP_Text>().text = page.text;
            p.GetComponentInChildren<Image>().sprite = page.image;

        }

        pagesParent = pageTemplate.transform.parent;
        pageTemplate.transform.SetParent(null);
    }


    private void SetCurrentPageIndex(int index)
    {
        if (index > pagesParent.childCount - 1)
            index = pagesParent.childCount - 1;
        else if (index < 0)
            index = 0;

        currentPageIndex = index;
    }

    private void ShowPage(int index)
    {
        UpdateButtons();

        for (int i = 0; i < pagesParent.childCount; i++)
        {
            pagesParent.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    private void UpdateButtons()
    {
        if (currentPageIndex == pagesParent.childCount - 1)
            nextButton.interactable = false;
        else
            nextButton.interactable = true;


        if (currentPageIndex == 0)
            backButton.interactable = false;
        else
            backButton.interactable = true;
    }

    public void NextPage()
    {
        SetCurrentPageIndex(currentPageIndex + 1);
        ShowPage(currentPageIndex);
    }

    public void PreviousPage()
    {
        SetCurrentPageIndex(currentPageIndex - 1);
        ShowPage(currentPageIndex);
    }


    public void StartGame()
    {

        HUD.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Level1GameplayLogic.StartGame();
        Level2GameplayLogic.StartGame();
        Level3GameplayLogic.StartGame();


    }

}
