using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    Image[] mapModules;
    bool[] states;

	void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.Pause.performed += _ => PauseOrResume();
        controls.Gameplay.Reset.performed += _ => ResetLevel();
        if(instance != null)
            GameObject.Destroy(instance);
        else
            instance = this;

        //DontDestroyOnLoad(this);
        GameObject temp = GameObject.FindGameObjectWithTag("Melita Indicator");
        if(temp)
            melitaIndicator = temp.GetComponent<Image>();

        mapModules = GameObject.FindGameObjectWithTag("Minimap").GetComponentsInChildren<Image>();
        ChangeMapVisibility(false); // should be default not visible
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

        SaveStates();
        Petrify.allowAbility = false;
        PowerUpRange.allowAbility = false;
        Player.allowMovement = false;
        Player.allowInteract = false;
        ChangeMapVisibility(true);
    }

    public void Resume() {
    	Debug.Log("resumed");
    	pauseMenu.SetActive(false);
    	pauseButton.SetActive(true);
    	Time.timeScale = 1f;
    	gameIsPaused = false;
        RestoreStates();
        ChangeMapVisibility(false);
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

    private void ChangeMapVisibility(bool visibility) {
        if(mapModules != null) {
            foreach(Image i in mapModules) {
                i.enabled = visibility;
            }
        }

        if(melitaIndicator)
            melitaIndicator.enabled = visibility;
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
