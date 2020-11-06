using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
	[SerializeField] private Sprite[] tipSprites;
	[SerializeField] private string[] tipTexts;
	[SerializeField] private Sprite previewImage = null;
	[SerializeField] private string text = null; // TODO make this text be able to be variable based on platform
	private GameObject popupBackground;
	private Image imagePreview;
	private TextMeshProUGUI textObject;
	private bool shownOnce = false;
	public PlayerControls controls;



	void Awake() {
		controls = new PlayerControls();
		controls.UI.Submit.performed += _ => Hide();

		popupBackground = GameObject.FindGameObjectWithTag("Tutorial Popup");
        // imagePreview = popupBackground.GetComponentInChildren<Image>(); // this doesn't work because this gets the image component in the parent first
        Image[] images = popupBackground.GetComponentsInChildren<Image>();       
        foreach(Image i in images) {
            if(i.gameObject.transform.parent != null) {
               imagePreview = i; //this gameObject is a child, because its transform.parent is not null
            }
        }

        textObject = popupBackground.GetComponentInChildren<TextMeshProUGUI>();
        ReplaceFields();
	}

    // Start is called before the first frame update
    void Start()
    {
    	Hide();
    }

    void UpdateFields() {
    	imagePreview.sprite = previewImage;
    	textObject.text = text;
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

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player") && !shownOnce) {
    		UpdateFields();
    		Show();
    		shownOnce = true; // mark it as shown so it doesn't appear again
    	}
    }

    void ReplaceFields() {
    	text = text.Replace("<PETRIFY>", PlatformSpecific.instance.petrifyString);
    }

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }
}
