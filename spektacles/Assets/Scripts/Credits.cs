using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	[SerializeField] GameObject defaultSelectedButton = null;

	void Start() {
        //DestroyDontDestroyOnLoad();
		EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
	}

    public void QuitGame() {
        Debug.Log("quitting game");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void LoadStartScene() {
        // playableDirector.time = 0;
        // playableDirector.Stop();
        // playableDirector.Evaluate();
        SceneManager.LoadScene(0);
        ResetStaticValues();
    }

    private void DestroyDontDestroyOnLoad() {
        Destroy (GameObject.Find("Unbreakable iPod"));
        Destroy (GameObject.Find("[Debug Updater]"));
        Destroy (GameObject.Find("FungusManager"));
        Destroy (GameObject.Find("~LeanTween"));

    }

    private void ResetStaticValues() {
        PauseMenu.gameIsPaused = false;
        PauseMenu.allowPause = true;
        // Destroy(PauseMenu.checkpointReachedDisplay);
        // Destroy(PauseMenu.checkpointRestartingDisplay);
        Destroy(PauseMenu.instance);
        QuitVerification.isOpen = false;
        Player.inCutscene = false;
        Player.allowMovement = true;
        Player.allowInteract = true;
    }
}
