using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JorcSoundController : MonoBehaviour
{
    public AudioSource footstepSounds;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void footstepLoopPlay(){
      footstepSounds.Play();
    }

    public void footstepLoopStop(){
      footstepSounds.Stop();
    }
}
