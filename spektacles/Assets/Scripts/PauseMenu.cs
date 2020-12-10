using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
	public static bool gameIsPaused = false;
    public static bool allowPause = true;

	public PlayerControls controls;
	[SerializeField] private GameObject pauseMenu = null;
	[SerializeField] private GameObject pauseButton = null;

    bool[] states;
    private Player player;
    public static GameObject checkpointReachedDisplay;
    public static GameObject checkpointRestartingDisplay;
    [SerializeField] private GameObject checkpointNoneReachedDisplay = null;

    [SerializeField] private GameObject quitVerification = null;

    public MixerControl mixerControl;

	void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.Pause.performed += _ => PauseOrResume();
        controls.Gameplay.RestartFromBeginning.performed += _ => RestartFromBeginning();
        controls.Gameplay.RestartFromCheckpoint.performed += _ => RestartFromCheckpoint();
        controls.Gameplay.Quit.performed += _ => VerifyQuit();

        if(instance != null)
            GameObject.Destroy(instance);
        else
            instance = this;

        pauseMenu.SetActive(true); // Set active during awake so during Awake() of Module.cs, they can find the map
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // set journal controls image to be for the proper device
        GameObject.Find("Journal Controls").GetComponent<Image>().sprite = GameAssets.instance.journalControls;

        pauseButton.GetComponent<Image>().sprite = GameAssets.instance.pauseButton;
        // quitVerification = GameObject.Find("Quit Verification");
        quitVerification.SetActive(false);
	}

    void Start() {
        checkpointReachedDisplay = GameObject.Find("Checkpoint Reached Display");
        checkpointRestartingDisplay = GameObject.Find("Checkpoint Restarting Display");

        // Only the pause button should be active at start
        if(pauseMenu != null) {
            pauseMenu.SetActive(false);
        } else {
            Debug.Log("pause menu is null! In PauseMenu.cs Start()");
        }

        if(checkpointReachedDisplay == null) {
            Debug.Log("no checkpointReachedDisplay!");
        } else {
            checkpointReachedDisplay.SetActive(false);
        }

        if(checkpointRestartingDisplay == null) {
            Debug.Log("no checkpointRestartingDisplay!");
        } else {
            checkpointRestartingDisplay.SetActive(false);
        }

        if(checkpointNoneReachedDisplay == null) {
            Debug.Log("no checkpointNoneReachedDisplay!");
        } else {
            checkpointNoneReachedDisplay.SetActive(false);
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

        mixerControl.muteSFX();

        // controls.Gameplay.Disable();
    }

    public void Resume() {
    	Debug.Log("resumed");
    	pauseMenu.SetActive(false);
    	pauseButton.SetActive(true);
    	Time.timeScale = 1f;
    	gameIsPaused = false;

        RestoreStates();
        mixerControl.unMuteSFX();

        // controls.Gameplay.Enable();
    }

    public void RestartFromBeginning(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Resume();
    }

    public void RestartFromCheckpoint() {
        if(player.checkpoint) {
            player.RestartLevel();
            Resume();
        } else {
            StartCoroutine(DisplayNoCheckpointsReached());
        }
    }

    private IEnumerator DisplayNoCheckpointsReached() {
        checkpointNoneReachedDisplay.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        checkpointNoneReachedDisplay.SetActive(false);
    }

    private void SaveStates() {
        states = new bool[5];
        states[0] = Petrify.allowAbility;
        states[1] = PowerUpRange.allowAbility;
        states[2] = Player.allowMovement;
        states[3] = Player.allowInteract;
        //states[4] = mixer.SFXVol;
    }

    private void RestoreStates() {
        Petrify.allowAbility = states[0];
        PowerUpRange.allowAbility = states[1];
        Player.allowMovement = states[2];
        Player.allowInteract = states[3];
        //mixer.SFXVol = states[4];
    }

    private void VerifyQuit() {
        quitVerification.SetActive(true);
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
