using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
	public static bool gameIsPaused = false;
    public static bool allowPause = true;

	public PlayerControls controls;
	[SerializeField] private GameObject pauseMenu = null;
	[SerializeField] private GameObject pauseButton = null;
    
    bool[] states;
    GameObject[] checkpoints = null;

	void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.Pause.performed += _ => PauseOrResume();
        controls.Gameplay.Reset.performed += _ => ResetLevel();

        if(instance != null)
            GameObject.Destroy(instance);
        else
            instance = this;

        pauseMenu.SetActive(true); // Set active during awake so during Awake() of Module.cs, they can find the map

        if(checkpoints == null) {
            // objects should be placed in order of increasing distance from the original player spawn point
            checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint"); 
        }
	}

    // Only the pause button should be active at start
    void Start() {
        if(pauseMenu != null) {
            pauseMenu.SetActive(false);
        } else {
            Debug.Log("pause menu is null! In PauseMenu.cs Start()");
        }
    }

    public void PauseOrResume() {
        if(allowPause && !Player.inCutscene) { // TODO do we need to put this check in the other functions too?
            Debug.Log("pause button pressed");
            if(gameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause() {
    	Debug.Log("paused");
    	pauseMenu.SetActive(true);
    	pauseButton.SetActive(false);
    	Time.timeScale = 0f;
    	gameIsPaused = true;

        SaveStates();
        Petrify.allowAbility = false;
        PowerUpRange.allowAbility = false;
        Player.allowMovement = false;
        Player.allowInteract = false;
        // ChangeMapVisibility(true); TODO change
    }

    public void Resume() {
    	Debug.Log("resumed");
    	pauseMenu.SetActive(false);
    	pauseButton.SetActive(true);
    	Time.timeScale = 1f;
    	gameIsPaused = false;
        RestoreStates();
        // ChangeMapVisibility(false); TODO change
    }

    public void ResetLevel(){
        GameObject checkpoint = GetFurthestCheckpointReached();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if(checkpoint)
            GameObject.FindGameObjectWithTag("Player").transform.position = checkpoint.transform.position;
    }

    private GameObject GetFurthestCheckpointReached() {
        for(int i = checkpoints.Length -1; i >= 0; i--) { // TODO can optimize to O(logN) time
            if(checkpoints[i].GetComponent<Checkpoint>().reached) {
                return checkpoints[i];
            }
        }
        return null;
    }


    private void SaveStates() {
        states = new bool[4];
        states[0] = Petrify.allowAbility;
        states[1] = PowerUpRange.allowAbility;
        states[2] = Player.allowMovement;
        states[3] = Player.allowInteract;
    }

    private void RestoreStates() {
        Petrify.allowAbility = states[0];
        PowerUpRange.allowAbility = states[1];
        Player.allowMovement = states[2];
        Player.allowInteract = states[3];
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
