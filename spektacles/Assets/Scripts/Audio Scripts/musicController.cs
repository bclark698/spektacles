using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicController : MonoBehaviour
{
  private static musicController controllerInstance;
  public AudioSource currentMusic;
  public AudioSource lvlMusic;
  public AudioSource cutMusic;


  //float fadeTime;
  public float musicVol;
  float currentVol;


  void Awake(){
    // DontDestroyOnLoad (transform.gameObject);
    DontDestroyOnLoad (this);
     currentMusic.clip = lvlMusic.clip;
     currentMusic.Play();
     if (controllerInstance == null) {
         controllerInstance = this;
     } else {
         Object.Destroy(gameObject);
     }
  }
  void Start()
  {
  //  currentMusic.clip = lvlMusic.clip;
  //  currentMusic.Play();
    //fadeTime = 100000;
    currentVol = .2f;
    musicVol = .2f;

  }

  // Update is called once per frame
  void Update()
  {
    currentMusic.volume = currentVol;

    if (Input.GetKeyDown(KeyCode.U)){
      StartCoroutine(MusicSwitch(cutMusic, 2, 5));
    }
    if (Input.GetKeyDown(KeyCode.J)){
      StartCoroutine(MusicSwitch(lvlMusic, 5, 2));
    }

    if (Input.GetKeyDown(KeyCode.M)){
      LoadNextScene();
      StartCoroutine(MusicSwitch(cutMusic, 1, 3));
    }
    if (Input.GetKeyDown(KeyCode.M)){
      LoadNextScene();
      StartCoroutine(MusicSwitch(cutMusic, 1, 3));
    }
    if (Input.GetKeyDown(KeyCode.N)){
      SceneManager.LoadScene(0);
      StartCoroutine(MusicSwitch(lvlMusic, 1, 3));
      Destroy(transform.gameObject);
    }

  }
  public void PlayLvlMusic(){
    currentMusic.clip = lvlMusic.clip;
    currentMusic.Play();
  }
  public void PlayCutMusic(){
    currentMusic.clip = cutMusic.clip;
    currentMusic.Play();
  }

  public void LoadNextScene(){
    SceneManager.LoadScene(1);
    StartCoroutine(MusicSwitch(cutMusic, 2, 5));
  }

  public void loadCustceneMusic(){
    //get current scene.
    //based on scene, have set music and time transistions set (controlled in this script)
    //prolly using a switch / case cause thats what I know to do 
    //Then trigger MusicSwitch() w/ the right values.
  }

  public IEnumerator MusicSwitch(AudioSource nextMusic, float transistionTimeDown, float transistionTimeUp){
    currentVol = musicVol;
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

    yield return null;
  }
}
