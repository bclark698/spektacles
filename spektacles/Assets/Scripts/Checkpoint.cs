using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public bool reached;
	private GameObject checkpointReachedDisplay;
	[SerializeField] private float displayTime = 0.3f;

	void Awake() {
		checkpointReachedDisplay = GameObject.Find("Checkpoint Reached Display");
		if(checkpointReachedDisplay == null) {
			Debug.Log("no checkpointReachedDisplay!");
		} else {
			checkpointReachedDisplay.SetActive(false);
		}
	}

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		reached = true;
    		StartCoroutine(DisplayReached());
    	}
    }

    IEnumerator DisplayReached() {
    	if(checkpointReachedDisplay == null) {
			Debug.Log("no checkpointReachedDisplay!");
		} else {
			checkpointReachedDisplay.SetActive(true);
	    	yield return new WaitForSeconds(displayTime);
	    	checkpointReachedDisplay.SetActive(false);
		}
    }
}
