using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Attention : MonoBehaviour
{
    private Sprite attentionSprite = null;
	private Sprite interactSprite = null;
	private SpriteRenderer indicator;
	public PlayerControls controls;
	bool targetInRange = false;
	[SerializeField] bool showOnStart = true;
	public UnityEvent attentionCleared;

	void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.EquipOrInteract.performed += _ => HideOnEnter();
		indicator = GetComponent<SpriteRenderer>();
        attentionSprite = GameAssets.instance.attention;
        interactSprite = GameAssets.instance.attentionInteract;
        
		if(showOnStart) {
			indicator.sprite = attentionSprite;
		} else {
            indicator.sprite = null;
        }
		if(attentionCleared == null) {
			attentionCleared = new UnityEvent();
		}
	}

    void HideOnEnter() {
    	if(targetInRange) {
    		TurnOff();
    	}
    }

    void OnTriggerEnter2D(Collider2D other) {
    	if(!other.CompareTag("Player")) {
    		return;
    	}
        if(other.CompareTag("Player")) {
            targetInRange = true;
        }
        if(indicator.sprite == attentionSprite) {
        	indicator.sprite = interactSprite;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            targetInRange = false;
        }
        if(indicator.sprite == interactSprite) {
        	indicator.sprite = attentionSprite;
        }
    }

    public void TurnOn() {
    	indicator.sprite = attentionSprite;
    }

    void TurnOff() {
    	indicator.sprite = null;
    	attentionCleared.Invoke();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
