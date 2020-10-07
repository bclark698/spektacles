using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FungusManager : MonoBehaviour
{
    //because fungus can't do everything ;_;
    private Player player;
    private GameObject mainCamera;
    private PlayerSoundController playerSounds;

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
    }

    public void LoseGlasses()
    {
        mainCamera.GetComponent<BoxBlur>().enabled = true;
        player.GetComponent<Animator>().SetBool("blind", true);
    }

    public void GainGlasses()
    {
        mainCamera.GetComponent<BoxBlur>().enabled = false;
        player.GetComponent<Animator>().SetBool("blind", false);
        playerSounds.aquireSound();
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame(){
        SceneManager.LoadScene(0);
    }

    public void TeleportPlayer(Vector2 newPos)
    {
        player.transform.position = newPos;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void WakeUp()
    {
        StartCoroutine(WakeUpAnim());
    }

    IEnumerator WakeUpAnim()
    {
        Debug.Log("u got a coroutine");
        //TODO: figure out how to actually trigger a 2d cutscene in unity lmfao

        yield return null;
    }
}
