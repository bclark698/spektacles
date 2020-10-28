using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
	public static bool gameIsPaused = false;
	public PlayerControls controls;
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject pauseButton;

	void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.Pause.performed += _ => PauseOrResume();
	}

    void PauseOrResume()
    {
        Debug.Log("pause button pressed");
        if(gameIsPaused) {
        	Resume();
        } else {
        	Pause();
        }
    }

    public void Pause() {
    	Debug.Log("paused");
    	pauseMenu.SetActive(true);
    	pauseButton.SetActive(false);
    	Time.timeScale = 0f;
    	gameIsPaused = true;
    }

    public void Resume() {
    	Debug.Log("resumed");
    	pauseMenu.SetActive(false);
    	pauseButton.SetActive(true);
    	Time.timeScale = 1f;
    	gameIsPaused = false;
    }

    public void HowToPlay() {
    	Debug.Log("how to play button pressed");
    	//TODO actually do something here
    }

    public void QuitGame() {
    	Debug.Log("quitting game");
    	#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
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
