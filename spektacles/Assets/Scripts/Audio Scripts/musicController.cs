using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicController : MonoBehaviour
{
  public AudioSource currentMusic;
  public AudioSource lvlMusic;
  public AudioSource cutMusic;

  float fadeTime;
  float musicVol;
  // Start is called before the first frame update
  void Awake(){
     DontDestroyOnLoad (transform.gameObject);
     currentMusic.clip = lvlMusic.clip;
     currentMusic.Play();
  }
  void Start()
  {
  //  currentMusic.clip = lvlMusic.clip;
  //  currentMusic.Play();
    fadeTime = 100000;
    musicVol = 1;
  }

  // Update is called once per frame
  void Update()
  {
    currentMusic.volume = musicVol;

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

  public IEnumerator MusicSwitch(AudioSource nextMusic, float transistionTimeDown, float transistionTimeUp){
    musicVol = 1;
   while (musicVol > 0){
      musicVol -= Time.deltaTime / transistionTimeDown;
      yield return musicVol;
    }

    yield return currentMusic.clip = nextMusic.clip;
    currentMusic.Play();
    while (musicVol < 1){
      musicVol += Time.deltaTime / transistionTimeUp;
      yield return musicVol;
    }

    yield return null;
  }
}
