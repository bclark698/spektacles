using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicController : MonoBehaviour
{
  private static musicController controllerInstance;
  public AudioSource currentMusic;
  public AudioSource homeMusic;
  public AudioSource homeCutMusic;
  public AudioSource lvl1Music;
  public AudioSource cut1Music;


  //float fadeTime;
  public float musicVol;
  float currentVol;

  private string currentLevel;


  void Awake(){

    DontDestroyOnLoad (this);

     if (controllerInstance == null) {
         controllerInstance = this;
     } else {
         Object.Destroy(gameObject);
     }


  }
  void Start()
  {

    currentVol = .2f;
    musicVol = .2f;

    currentLevel = SceneManager.GetActiveScene().name;
    LoadMusic();
    currentMusic.Play();
  }

  // Update is called once per frame
  void Update()
  {

    currentMusic.volume = currentVol;

  }
  public void LoadMusic(){
    switch (currentLevel){

      case "Home_Test":
      currentMusic.clip = homeMusic.clip;
      break;
      case "Floor1 NEW":
      currentMusic.clip = lvl1Music.clip;
      break;
      case "TEST Floor1 NEW":
      currentMusic.clip = lvl1Music.clip;
      break;



    }

  }
  public void LoadHallScene(){
    SceneManager.LoadScene(1);
    StartCoroutine(MusicSwitch(lvl1Music, 2, 5));
  }

  public void LoadNextScene(){
    SceneManager.LoadScene(1);
    StartCoroutine(MusicSwitch(cut1Music, 2, 5));
  }

  public void loadCustceneMusic(){
    switch (currentLevel){

      case "Floor1 NEW":
      Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cut1Music, 2, 4));
      break;


    }
    //prolly using a switch / case cause thats what I know to do
    //Then trigger MusicSwitch() w/ the right values.
  }

  public IEnumerator MusicSwitch(AudioSource nextMusic, float transistionTimeDown, float transistionTimeUp){
    Debug.Log("starting switch");
  currentVol = musicVol;
   while (currentVol > 0){
      currentVol -= Time.deltaTime / transistionTimeDown;
      Debug.Log("switching");
      yield return currentVol;
    }

    yield return currentMusic.clip = nextMusic.clip;
    currentMusic.Play();
    while (currentVol < musicVol){
      currentVol += Time.deltaTime / transistionTimeUp;
      yield return currentVol;
    }

    yield return null;
  }
}
