﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
	public static bool gameIsPaused = false;
    public static bool allowPause = true;

	public PlayerControls controls;
	[SerializeField] private GameObject pauseMenu = null;
	[SerializeField] private GameObject pauseButton = null;
    Image melitaIndicator;

	void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.Pause.performed += _ => PauseOrResume();
        controls.Gameplay.Reset.performed += _ => ResetLevel();
        if(instance != null)
            GameObject.Destroy(instance);
        else
            instance = this;

        //DontDestroyOnLoad(this);

        melitaIndicator = GameObject.FindGameObjectWithTag("Melita Indicator").GetComponent<Image>();
        melitaIndicator.enabled = false; // should be default off
	}

    public void PauseOrResume()
    {
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
        Petrify.allowAbility = false;
        PowerUpRange.allowAbility = false;
        Player.allowMovement = false;
        Player.allowInteract = false;
        melitaIndicator.enabled = true;
    }

    public void Resume() {
    	Debug.Log("resumed");
    	pauseMenu.SetActive(false);
    	pauseButton.SetActive(true);
    	Time.timeScale = 1f;
    	gameIsPaused = false;
        Petrify.allowAbility = true;
        PowerUpRange.allowAbility = true;
        Player.allowMovement = true;
        Player.allowInteract = true;
        melitaIndicator.enabled = false;
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

    public void ResetLevel(){
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
