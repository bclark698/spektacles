using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
	// each tip should have an image and some text, so these should be equal length arrays
	[SerializeField] private Sprite[] tipSprites = null;
	[SerializeField] private string[] tipTexts = null;

	private GameObject popupBackground;
	private Image imagePreview;
	private TextMeshProUGUI tipText;
	private TextMeshProUGUI pageNumText;
	private Image rightButton;
	private Image leftButton;

	private bool shownOnce = false;
	public PlayerControls controls;
	private int tipNum = 0;

	void Awake() {
		controls = new PlayerControls();
		controls.UI.Submit.performed += _ => Hide();
		controls.UI.Navigate.performed += context => DeterminePrevOrNext(context);

		popupBackground = GameObject.FindGameObjectWithTag("Tutorial Popup");
        imagePreview = popupBackground.transform.Find("Image Preview").GetComponent<Image>();
        tipText = popupBackground.transform.Find("Tip Text").GetComponent<TextMeshProUGUI>();
        pageNumText = popupBackground.transform.Find("Page Num").GetComponent<TextMeshProUGUI>();
        rightButton = popupBackground.transform.Find("Right Button").GetComponent<Image>();
        leftButton = popupBackground.transform.Find("Left Button").GetComponent<Image>();
        leftButton.sprite = GameAssets.instance.arrowPrev;

        ReplaceFields();
	}

	void DeterminePrevOrNext(InputAction.CallbackContext context) {
		if(IsShowing()) {
			float x = context.ReadValue<Vector2>().x;
			if(x > 0) {
				NextTip();
			} else if(x < 0) {
				PrevTip();
			}
		}
	}

    /* The tutorial popup gameobject should be enabled in the UI by default in order 
    to properly retrieve the variables, and will be hidden on start */
    void Start() {
    	Hide();
    }

    void UpdateFields() {
    	imagePreview.sprite = tipSprites[tipNum];
    	tipText.text = tipTexts[tipNum];
    	UpdateButtons();
    	UpdatePageNum();
    }

    void Show() {
    	popupBackground.SetActive(true);
    	Petrify.allowAbility = false;
        PowerUpRange.allowAbility = false;
        Player.allowMovement = false;
        PauseMenu.allowPause = false;
    }

    void Hide() {
    	popupBackground.SetActive(false);
    	Petrify.allowAbility = true;
        PowerUpRange.allowAbility = true;
        Player.allowMovement = true;
        PauseMenu.allowPause = true;
    }

    bool IsShowing() {
    	return popupBackground.activeSelf;
    }

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player") && !shownOnce) {
    		UpdateFields();
    		Show();
    		shownOnce = true; // mark it as shown so it doesn't appear again
    	}
    }

    void ReplaceFields() {
    	for(int i = 0; i < tipTexts.Length; i++) {
    		tipTexts[i] = tipTexts[i].Replace("<PETRIFY>", GameAssets.instance.petrifyString);
    	}
    }

    int NumTips() {
    	return tipSprites.Length;
    }

    void NextTip() {
    	if(tipNum < NumTips()-1) {
    		tipNum++;
    		UpdateFields();
    	}
    }

    void PrevTip() {
    	if(tipNum > 0) {
    		tipNum--;
    		UpdateFields();
    	}
    }

    void UpdateButtons() {
    	Image leftBtnImg = leftButton.GetComponent<Image>();
        var tempColor = leftBtnImg.color;
    	if(NumTips() > 1) {
    		if(tipNum == 0) { //first page
    			tempColor.a = 0f;
	    	} else {
	    		tempColor.a = 1f;
	    	}
	    	
	    	if(tipNum == NumTips()-1) {
	    		rightButton.sprite = GameAssets.instance.tipExit;
	    	} else {
	    		rightButton.sprite = GameAssets.instance.arrowNext;
	    	}
    	} else {
    		tempColor.a = 0f;
    		rightButton.sprite = GameAssets.instance.tipExit;
    	}
    	leftBtnImg.color = tempColor;
    }

    void UpdatePageNum() {
    	if(NumTips() > 1) {
    		pageNumText.text = (tipNum+1)+"/"+NumTips();
		} else {
			pageNumText.text = "";
		}
    	
    }

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }
}
