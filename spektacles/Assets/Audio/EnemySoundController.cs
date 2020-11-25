using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundController : MonoBehaviour
{

    public AudioSource turnStoneSound;
    public AudioSource stoneCrackSound;
    public AudioSource breakFreeSound;
    public AudioSource smokePoofSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playTurnStoneSound(){
      turnStoneSound.Play();
    }

    public void playStoneCrackSound(){
      stoneCrackSound.Play();
    }

    public void playBreakFreeSound(){
      breakFreeSound.Play();
    }

    public void playSmokePoof(){
      smokePoofSound.Play();
    }

}
