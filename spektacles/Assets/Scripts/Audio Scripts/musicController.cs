using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicController : MonoBehaviour
{
  private static musicController controllerInstance;
  public AudioSource startMusic;
  public AudioSource currentMusic;
  public AudioSource homeMusic;
  public AudioSource homeCutMusic;
  public AudioSource busMusic;
  public AudioSource busCutMusic;
  public AudioSource lvl1Music;
  public AudioSource cut1Music;


  //float fadeTime;
  public float musicVol;
  float currentVol;

  private string currentLevel;

  void OnEnable()
  {
    //  Debug.Log("OnEnable called");
      SceneManager.sceneLoaded += OnSceneLoaded;
  }

  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
     {
       currentLevel = SceneManager.GetActiveScene().name;
       Debug.Log("current level is ");
       Debug.Log(currentLevel);
       LoadMusic();
       currentMusic.Play();
     }

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
    currentVol = musicVol;


    currentLevel = SceneManager.GetActiveScene().name;
    LoadMusic();
    currentMusic.Play();
  }



  // Update is called once per frame
  void Update()
  {
    currentLevel = SceneManager.GetActiveScene().name;
    currentMusic.volume = currentVol;

  }
  public void LoadMusic(){
    switch (currentLevel){

      case "Home":
      currentMusic.clip = homeMusic.clip;
      break;
      case "Bus":
      currentMusic.clip = busMusic.clip;
      break;
      case "School Level 1":
      currentMusic.clip = lvl1Music.clip;
      break;
      case "Full School Scene":
      currentMusic.clip = lvl1Music.clip;
      break;
      case "Sohee TEST - Start Screen":
      currentMusic.clip = startMusic.clip;
      break;

    }
    //Debug.Log("you're now listening to the sweet tunes of... ");
    //Debug.Log(currentMusic.clip);
  }

  public void loadCustceneMusic(){
    //Debug.Log("should be switching");
    switch (currentLevel){

      case "Floor1 NEW":
      Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cut1Music, 2, 4));
      break;
      case "Bus":
      Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(busCutMusic, 2, 4));
      break;
      case "School level 1":
      Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cut1Music, 2, 4));
      break;
      case "Full School Scene":
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
