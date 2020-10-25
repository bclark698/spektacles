using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUI : MonoBehaviour
{
	[SerializeField] private string targetTag = "Player";
	[SerializeField] private ControlsIndicator.Icon icon = ControlsIndicator.Icon.Interact;
	[SerializeField] private bool showIndicatorOverPlayer = false;
	private ControlsIndicator indicator;

	void Start() {
		if(GetComponent<SpriteRenderer>() == null) {
			showIndicatorOverPlayer = true;
		}
		if(showIndicatorOverPlayer) {
			indicator = GameObject.FindGameObjectWithTag(targetTag).GetComponent<ControlsIndicator>();
		} else {
			indicator = GetComponent<ControlsIndicator>();
		}
	}

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag(targetTag)) {
    		indicator.Show(icon);
    	}
    }

    void OnTriggerExit2D(Collider2D other) {
    	if(other.CompareTag(targetTag)) {
    		indicator.Hide();
    	}
    }
}
