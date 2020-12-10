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
  public AudioSource homeLoop;
  public AudioSource busMusic;
  public AudioSource busLoop;
  public AudioSource lvl1Music;
  public AudioSource lvl1Loop;
  public AudioSource lvl2Music;
  public AudioSource lvl2Loop;
  public AudioSource lvl3Music;
  public AudioSource lvl3Loop;
  public AudioSource cutMusic;
  public AudioSource cutLoop;

  private AudioSource currentMusicLoop;
  //private bool musicLooped = true;


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
       Debug.Log(currentLevel);
       LoadMusic();
       //currentMusic.Play();
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


    if (Time.timeSinceLevelLoad >= 20){

    if (!currentMusic.isPlaying){
      currentMusic.clip = currentMusicLoop.clip;
      currentMusic.Play();
      currentMusic.loop = true;
    }
  }



  }
  public void LoadMusic(){
    switch (currentLevel){

      case "Home":
      //currentMusic.clip = homeMusic.clip;
      StartCoroutine(MusicSwitch(homeMusic, homeLoop, 1, 1, .25f));
      break;
      case "Bus":
    //  currentMusic.clip = busMusic.clip;
      StartCoroutine(MusicSwitch(busMusic, busLoop, 1, 2, .2f));
      break;
      case "School Level 1":
      //currentMusic.clip = lvl1Music.clip;
      StartCoroutine(MusicSwitch(lvl1Music, lvl1Loop, 1, 2, .25f));
      break;
      case "School Level 2":
    //  currentMusic.clip = lvl2Music.clip;
      StartCoroutine(MusicSwitch(lvl2Music, lvl2Loop, 1, 2, .2f));
      break;
      case "School Level 3":
    //  currentMusic.clip = lvl3Music.clip;
      StartCoroutine(MusicSwitch(lvl3Music, lvl3Loop, 1, 2, .23f));
      break;
      case "Sohee TEST - Start Screen":
      StartCoroutine(MusicSwitch(busMusic, busLoop, 1, 2, .2f));
      break;
      case "Credits":
      Time.timeScale = 1f;
      StartCoroutine(MusicMute(10));
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
      StartCoroutine(MusicSwitch(cutMusic, cutLoop, 2, 4, .4f));
      break;
      case "Bus":
      //Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cutMusic, cutLoop, 2, 4, .4f));
      break;
      case "School Level 1":
      //Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cutMusic, cutLoop, 2, 4, .4f));
      break;
      case "School Level 2":
      //Debug.Log("switching to cut2Music");
      StartCoroutine(MusicSwitch(cutMusic, cutLoop, 2, 4, .4f));
      break;
      case "School Level 3":
      //Debug.Log("switching to cut3Music");
      StartCoroutine(MusicSwitch(cutMusic, cutLoop, 2, 4, .4f));
      break;
      case "Full School Scene":
      //Debug.Log("switching to cut1Music");
      StartCoroutine(MusicSwitch(cutMusic, cutLoop, 2, 4, .4f));
      break;
      case "Firedrill Test":
      StartCoroutine(MusicSwitch(cutMusic, cutLoop, 2, 4, .4f));
      break;


    }

  }

  public IEnumerator MusicSwitch(AudioSource nextMusic, AudioSource loopMusic, float transistionTimeDown, float transistionTimeUp, float vol){
    currentVol = musicVol;
    musicVol = vol;
    currentMusic.loop = false;
    currentMusicLoop = loopMusic;

   while (currentVol > 0){
      currentVol -= Time.deltaTime / transistionTimeDown;
      yield return currentVol;
    }

    yield return currentMusic.clip = nextMusic.clip;
    currentMusic.Play();
    while (currentVol < musicVol){
      currentVol += Time.deltaTime / transistionTimeUp;
      yield return currentVol;
    }
    //StartCoroutine(LoopMusic(loopMusic));

    yield return null;
  }



  public IEnumerator LoopMusic(AudioSource loopMusic){
    yield return currentMusic.clip = loopMusic.clip;
    currentMusic.Play();
    currentMusic.loop = true;
  }

  public IEnumerator MusicMute(float transistionTimeDown){
    currentVol = musicVol;
    while (currentVol > 0){
      currentVol -= Time.deltaTime / transistionTimeDown;
      Debug.Log (currentVol);
      yield return currentVol;
    }
  }
}
