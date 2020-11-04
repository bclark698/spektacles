using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
	public static bool gameIsPaused = false;

	public PlayerControls controls;
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject pauseButton;
    TextMeshProUGUI objectiveText;

	void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.Pause.performed += _ => PauseOrResume();
        if(instance != null)
            GameObject.Destroy(instance);
        else
            instance = this;
         
        DontDestroyOnLoad(this);

        objectiveText = GameObject.FindGameObjectWithTag("Objective Text").GetComponent<TextMeshProUGUI>();
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
        objectiveText.enabled = true;
    }

    public void Resume() {
    	Debug.Log("resumed");
    	pauseMenu.SetActive(false);
    	pauseButton.SetActive(true);
    	Time.timeScale = 1f;
    	gameIsPaused = false;
        objectiveText.enabled = false;
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
