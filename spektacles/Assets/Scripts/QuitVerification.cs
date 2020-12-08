using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class QuitVerification : MonoBehaviour
{
	public static bool isOpen = false;
	[SerializeField] GameObject defaultSelectedButton;

    public void LoadCreditsScene() {
        SceneManager.LoadScene("Credits");
    }

    public void CloseQuitVerification() {
        // quitVerification.SetActive(false);
        gameObject.SetActive(false);
        isOpen = false;
    }

    private void OnEnable() {
    	isOpen = true;
    	EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }

    private void OnDisable() {
    	isOpen = false;
    }
}
