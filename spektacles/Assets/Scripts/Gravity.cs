﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Rigidbody2D playerRB; //var for player rigidbody
    [HideInInspector]
    public bool inRange = false; //if player is in range
    [HideInInspector]
    public CircleCollider2D gravityRange;

    [SerializeField] private float gravitationalForce = 90; //adjust gravity

    public AudioSource sirenPullSound;
    public AudioSource sirenBlockedSound;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>(); //grab player RB
        gravityRange = gameObject.GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        if (inRange)
        {
            // make a vector 2 of wherever the player is
            var directionOfPlayer = ((Vector2)transform.position - playerRB.position).normalized;

            // adds force to the player to pull them towards siren
            playerRB.AddForce(directionOfPlayer * gravitationalForce);


        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //once u enter the range
    {
        if (collision.CompareTag("Player") && collision.GetComponent<Player>().powerUp != PowerUp.PowerUpType.EarPlugs)
        {
            inRange = true; //start pulling player
            sirenPullSound.Play();
        }
        else if ( collision.CompareTag("Player") && collision.GetComponent<Player>().powerUp == PowerUp.PowerUpType.EarPlugs){
            sirenBlockedSound.Play();
        }

    }

    private void OnTriggerExit2D(Collider2D collision) //once u leave the range
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false; //stop pulling player

            sirenBlockedSound.Stop();
            sirenPullSound.Stop();
        }
    }
}