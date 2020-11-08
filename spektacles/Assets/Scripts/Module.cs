using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
	private Transform miniMapLocation = null;
	private GameObject melitaIndicator;
	private string prefix = "School- ";

	void Start() {
		melitaIndicator = GameObject.FindGameObjectWithTag("Melita Indicator");
		string parentName = transform.parent.name;
		string pointerName = "Pointer "+parentName.Substring(prefix.Length);
		Debug.Log(parentName +" "+pointerName);
		miniMapLocation = GameObject.Find(pointerName).GetComponent<Transform>();
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
    	melitaIndicator.transform.position = miniMapLocation.position;
    }
}
