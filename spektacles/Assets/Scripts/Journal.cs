﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class Journal : MonoBehaviour
{
	public PlayerControls controls;
	// Two-dimensional list. # rows = # tabs, # columns for each row is number of page pairs for each tab
    private List<List<GameObject>> pages;
    private List<GameObject> tabs = null;
    private int tabNum = 0;
    private int pageNum = 0;
    private Image journalAppearance;
    private Text pageNumLeft;
    private Text pageNumRight;
    [SerializeField] private string pageNumDecoration = ""; // goes on the left and right side of the page number
    private string pageNumDecorationReversed = "";

	void Awake() {
		controls = new PlayerControls();
		controls.UI.Navigate.performed += context => NavigateJournal(context);

        journalAppearance = GetComponent<Image>();

        pages = new List<List<GameObject>>();
        tabs = gameObject.GetChildren();

        foreach(GameObject go in tabs) {
        	pages.Add(go.GetChildren());
        }

        pageNumLeft = GameObject.Find("Page Num Left").GetComponent<Text>();
        pageNumRight = GameObject.Find("Page Num Right").GetComponent<Text>();

        pageNumDecorationReversed = new string(pageNumDecoration.Reverse().ToArray());
	}

	void NavigateJournal(InputAction.CallbackContext context) {
        if(PauseMenu.gameIsPaused) {
            float x = context.ReadValue<Vector2>().x;
            if(x > 0) {
                NextPage();
            } else if(x < 0) {
                PrevPage();
            }

            float y = context.ReadValue<Vector2>().y; // directions are reversed
            if(y < 0) {
                NextTab();
            } else if(y > 0) {
                PrevTab();
            }

            UpdateJournalView();
        }
    }

    int NumTabs() {
        return tabs.Count;
    }

    int NumPages() {
        return pages[tabNum].Count;
    }

    void NextPage() {
        if(pageNum < NumPages() - 2) {
        	SetCurPairPagesVisibility(false);
            pageNum += 2;
            SetCurPairPagesVisibility(true);
        }
    }

    void PrevPage() {
        if(pageNum > 0) {
        	SetCurPairPagesVisibility(false);
        	pageNum -= 2;
        	SetCurPairPagesVisibility(true);
        }
    }

    void NextTab() {
        if(tabNum < NumTabs()-1) {
        	SetCurPairPagesVisibility(false);
        	tabs[tabNum].SetActive(false);
            tabNum++;
            tabs[tabNum].SetActive(true);
            SetFirstPagePairVisible();
        }
    }

    void PrevTab() {
        if(tabNum > 0) {
        	SetCurPairPagesVisibility(false);
        	tabs[tabNum].SetActive(false);
            tabNum--;
            tabs[tabNum].SetActive(true);
            SetFirstPagePairVisible();
        }
    }

    void SetFirstTabVisible() {
    	tabNum = 0;
    	tabs[0].SetActive(true);
    	for(int i = 1; i < tabs.Count; i++) {
    		tabs[i].SetActive(false);
    	}
    }

    // Each tab must have at least two pages
    void SetFirstPagePairVisible() {
    	pageNum = 0;
    	List<GameObject> curTabPages = pages[tabNum];
    	curTabPages[0].SetActive(true);
    	curTabPages[1].SetActive(true);
    	for(int i = 2; i < curTabPages.Count; i++) {
    		curTabPages[i].SetActive(false);
    	}
    }

    void SetCurPairPagesVisibility(bool visibility) {
    	pages[tabNum][pageNum].SetActive(visibility);
        if(pageNum+1 < NumPages()) {
    		pages[tabNum][pageNum+1].SetActive(visibility);
    	}
    }

    void UpdateJournalView() {
        UpdatePageAppearance();
        UpdateControlsView();
        UpdatePageNum();
    }

    void UpdatePageAppearance() {
    	journalAppearance.sprite = tabs[tabNum].GetComponent<Image>().sprite;
    }

    void UpdatePageNum() {
        pageNumLeft.text = pageNumDecoration + (pageNum + 1) + pageNumDecorationReversed;
        if(pageNum+1 < NumPages()) {
        	pageNumRight.text = pageNumDecoration + (pageNum + 2).ToString() + pageNumDecorationReversed;
    	} else {
    		pageNumRight.text = "";
    	}
        
    }

    void UpdateControlsView() {
        // if(NumPages() < 2)
    }

    public void QuitGame() {
    	Debug.Log("quitting game");
    	#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void OnEnable()
    {
        controls.Enable();
        SetFirstTabVisible();
        SetFirstPagePairVisible();
        UpdateJournalView();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}