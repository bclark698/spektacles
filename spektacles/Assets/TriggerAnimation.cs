using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    private Animator animControl;
    private AudioSource canSound;

    void Start()
    {
        animControl = gameObject.GetComponent<Animator>();
        canSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other){
      if (other.tag == "Player"){
        if(animControl.GetCurrentAnimatorStateInfo(0).IsName("New State")){
          animControl.SetTrigger("OpenCan");
          if (!canSound.isPlaying){
          canSound.Play();
        }
}

      }

    }
}
