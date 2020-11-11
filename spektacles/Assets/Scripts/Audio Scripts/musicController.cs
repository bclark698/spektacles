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
  public AudioSource lvl2Music;
  public AudioSource cut2Music;
  public AudioSource lvl3Music;
  public AudioSource cut3Music;


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
      //currentMusic.clip = lvl1Music.clip;
      StartCoroutine(MusicSwitch(lvl1Music, 1, 2, .1f));
      break;
      case "School Level 2":
      currentMusic.clip = lvl2Music.clip;
      break;
      case "School Level 3":
      currentMusic.clip = lvl3Music.clip;
      break;
      case "Full School Scene":
      currentMusic.clip = lvl1Music.clip;
      break;
      case "Sohee TEST - Start Screen":
      currentMusic.clip = startMusic.clip;
      break;
      case "Firedrill Test":
      currentMusic.clip = lvl3Music.clip;
      break;


    }
    //Debug.Log("you're now listening to the sweet tunes of... ");
    //Debug.Log(currentMusic.clip);
  }

  public void loadCustceneMusic(){
    Debug.Log("should be switching");
    switch (currentLevel){

      case "Floor1 NEW":
      //Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cut1Music, 2, 4, .4f));
      break;
      case "Bus":
      //Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(busCutMusic, 2, 4, .4f));
      break;
      case "School Level 1":
      //Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cut1Music, 2, 4, .4f));
      break;
      case "School Level 2":
      //Debug.Log("switching to cut2Music");
      StartCoroutine(MusicSwitch(cut2Music, 2, 4, .4f));
      break;
      case "School Level 3":
      //Debug.Log("switching to cut3Music");
      StartCoroutine(MusicSwitch(cut3Music, 2, 4, .4f));
      break;
      case "Full School Scene":
      //Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cut1Music, 2, 4, .4f));
      break;
      case "Firedrill Test":
      StartCoroutine(MusicSwitch(cut3Music, 2, 4, .4f));
      break;


    }
    //prolly using a switch / case cause thats what I know to do
    //Then trigger MusicSwitch() w/ the right values.
  }

  public IEnumerator MusicSwitch(AudioSource nextMusic, float transistionTimeDown, float transistionTimeUp, float vol){
    Debug.Log("starting switch");
    musicVol = vol;
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
