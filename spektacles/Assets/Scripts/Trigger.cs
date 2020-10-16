using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
	[SerializeField] private string tag = "Player";
	[SerializeField] private ShowInteractIndicator.Icon icon;

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag(tag)) {
    		other.gameObject.GetComponent<ShowInteractIndicator>().Show(icon);
    	}
    }

    void OnTriggerExit2D(Collider2D other) {
    	if(other.CompareTag(tag)) {
    		other.gameObject.GetComponent<ShowInteractIndicator>().Hide();
    	}
    }
}
