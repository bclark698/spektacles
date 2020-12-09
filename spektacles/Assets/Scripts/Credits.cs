using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	[SerializeField] GameObject defaultSelectedButton = null;

	void Start() {
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
    	SceneManager.LoadScene(0);
    }
}
