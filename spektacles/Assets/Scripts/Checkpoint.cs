using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public bool reached;
	// private GameObject checkpointReachedDisplay;
	// private GameObject checkpointRestartingDisplay;
	private float displayTime = 1.5f;
	private bool displayedOnce;
	private Player player;
	[SerializeField] private int orderNum = 0;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		reached = true;
    		if(!displayedOnce) {
    			StartCoroutine(DisplayReached());
    		}
    		player.checkpoint = GetFurthestCheckpointReached();
    	}
    }

    // aesthetic TODO: make fade in and out
    private IEnumerator DisplayReached() {
    	displayedOnce = true;
    	if(PauseMenu.checkpointReachedDisplay == null) {
			Debug.Log("no checkpointReachedDisplay!");
		} else {
			PauseMenu.checkpointReachedDisplay.SetActive(true);
	    	yield return new WaitForSeconds(displayTime);
	    	if(player.checkpoint == this) {
	    		PauseMenu.checkpointReachedDisplay.SetActive(false);
	    	}
		}
    }

    public Checkpoint GetFurthestCheckpointReached() {
    	Checkpoint previousCheckpoint = player.checkpoint;
        if(previousCheckpoint && orderNum < previousCheckpoint.orderNum) {
        	return previousCheckpoint;
        } else {
        	return this;
        }
    }

    public void RestartAtCheckpoint() {
    	player.restartPoint = transform;
    	player.transform.position = transform.position;
    	StartCoroutine(DisplayRestarting());
    }

    // aesthetic TODO: make fade in and out
    private IEnumerator DisplayRestarting() {
    	PauseMenu.checkpointRestartingDisplay.SetActive(true);
    	yield return new WaitForSeconds(displayTime);
    	PauseMenu.checkpointRestartingDisplay.SetActive(false);
    }
}
