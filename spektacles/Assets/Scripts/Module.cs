using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
	[SerializeField] private Transform miniMapLocation = null; // drag into inspector
	private GameObject melitaIndicator;

	void Start() {
		melitaIndicator = GameObject.FindGameObjectWithTag("Melita Indicator");
	}

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		Debug.Log("module enter!");
    		UpdateMiniMap();
    	}
    	if(other.CompareTag("End")) {
    		Debug.Log("irving in this module!"); // doesnt work
    	}
    }

    void OnTriggerStay2D(Collider2D other) {
    	if(other.CompareTag("End")) {
    		Debug.Log("irving in module STAY"); // doesnt work
    	}
    }

    void UpdateMiniMap() {
    	// Debug.Log("mini loc pos "+miniMapLocation.position);
    	melitaIndicator.transform.position = miniMapLocation.position;
    }
}
