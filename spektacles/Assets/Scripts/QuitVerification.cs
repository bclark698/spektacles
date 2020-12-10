using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class QuitVerification : MonoBehaviour
{
	public static bool isOpen = false;
	[SerializeField] GameObject defaultSelectedButton = null;
    public PlayerControls controls; // pausemenu controls

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
        if(controls != null)
            controls.Gameplay.Disable();
    	EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
    }

    private void OnDisable() {
    	isOpen = false;
        if(controls != null)
            controls.Gameplay.Enable();
    }
}
