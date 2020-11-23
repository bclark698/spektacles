using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;
using Fungus;

public class FungusManager : MonoBehaviour
{
    //because fungus can't do everything ;_;
    private Player player;
    private GameObject mainCamera;
    private PlayerSoundController playerSounds;
    private Animator playerAnimator;

    //private FungusManager managerInstance;

    //void Awake() //cant get this to work, will come back to it maybe
    //{
    //    DontDestroyOnLoad(this);

    //    if (managerInstance == null)
    //    {
    //        managerInstance = this;
    //    }
    //    else
    //    {
    //        Object.Destroy(gameObject);
    //    }
    //}

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    // the reason this doesn't use the player function is bc it doesn't need to petrify
    public void ToggleBlur(bool on)
    {
        BoxBlur blur = mainCamera.GetComponent<BoxBlur>();

        blur.enabled = on;
    //TODO: add the animations
        playerAnimator.SetBool("blind", on);
        if (!on)
            playerSounds.AcquireSound();

    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayCutscene(PlayableDirector pd)
    {
        pd.Play();
    }

    public void TogglePlayer(bool on)
    {
        Petrify petrify = player.GetComponentInChildren<Petrify>();
        player.enabled = on;
        petrify.enabled = on;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleFlavorText(GameObject other, bool on)
    {
        Flowchart flow = other.GetComponent<Flowchart>();
        flow.enabled = on;
    }


}
