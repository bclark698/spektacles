using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSoundController : MonoBehaviour
{

    private AudioSource doorSound;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        doorSound = gameObject.GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider){
      if (collider.tag == "Player"){
      doorSound.Play();
      }
    }
}
