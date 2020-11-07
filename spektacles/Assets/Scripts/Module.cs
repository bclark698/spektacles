using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		Debug.Log("module enter!");
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
    	//
    }
}
