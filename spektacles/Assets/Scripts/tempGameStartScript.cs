using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class tempGameStartScript : MonoBehaviour
{
    public PlayerControls controls;

    // Start is called before the first frame updat
    void Awake()
    {
      controls = new PlayerControls();
      controls.Gameplay.Start.performed += _ => loadHome();
    }


        public void loadHome(){
          Debug.Log("starting game");
           SceneManager.LoadScene(1);
        }

        // Called when the Player object is enabled
        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }
}
