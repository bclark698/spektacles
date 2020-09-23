﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementVelocity;
    public float moveSpeed;
    private Animator anim;
    [HideInInspector]
    public GameObject eyeglasses;
    private int lives = 2; //one for w/ glasses, one for without


    //These won't actually be like this in the future - I'll just have one playerAudioSource;
    // it'll be clean, promise
    // But for now, just assist the showing of functionality
    //public PlayerSoundController playerSounds;
    private PlayerSoundController playerSounds;
    /*
    public AudioSource tempPickupNoise;
    public AudioSource tempSprayNoise;
    public AudioSource hitNoise;
    */

    // powerUp variables
    public PowerUp.PowerUpType powerUp = PowerUp.PowerUpType.None;
    public Transform powerUpRangePos;
    public LayerMask whatIsEnemies;
    public float powerUpRange;
    public GameObject powerUpObj; //TODO make private

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();

        transform.GetChild(0).gameObject.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVelocity = moveInput.normalized * moveSpeed;

        //Movement Animations - I am positive u can handle this in one line (kat)
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.RightArrow)))
        {
            //Checks for Up,Down,Left,Right Movement and sets the walking boolean in the Animator to true to trigger the walking animation
            anim.SetBool("walking", true);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.D))))
        {
            //Same as above for WASD
            anim.SetBool("walking", true);
        }
        if (!Input.anyKey)
        {
            //If the player is not pressing any key at all, sets walking to false
            anim.SetBool("walking", false);
        }

        /* Important to use.GetKeyDown(KeyCode.P) instead of.GetKey(KeyCode.P) because
         * GetKey triggers more than once */
        if (Input.GetKeyDown(KeyCode.P))
        {
            UsePowerUp();
        }
    }

    /* Handle powerUp. A held powerUp still gets used (wasted) even if no enemies are
     * in range to let player try out using powerups. */
    void UsePowerUp()
    {
        if(powerUp != PowerUp.PowerUpType.None)
        {
            // get all the enemies within our PowerUpRange
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(powerUpRangePos.position, powerUpRange, whatIsEnemies);

                // don't check enemies unless enemies are in range
                else if (enemiesInRange.Length > 0)
                {
                    powerUpObj.GetComponent<PowerUp>().Use();
                    // tempSprayNoise.Play();
                    // TODO put this noise in bug spray powerup/pixie code

                    // have each enemy determine how to handle this powerup being used on them
                    for (int i = 0; i < enemiesInRange.Length; i++)
                    {
                        enemiesInRange[i].GetComponent<Enemy>().HandlePowerUp(powerUp);
                    }
                    // set player back to holding no powerup
                    powerUp = PowerUp.PowerUpType.None;
                }
            }
            // do some dashy shit
            else if(powerUp == PowerUp.PowerUpType.Dash)
            {
               // dashyShit();
                powerUpObj.GetComponent<PowerUp.Dash>().Use();
            }
            else
            {
                enemiesInRange[i].GetComponent<Enemy>().HandlePowerUp(temp);
            }
        }
    }

    /* For testing purposes, this draws red line around the player's power up range.
     * This has no effect during gameplay, so we can leave this in. */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(powerUpRangePos.position, powerUpRange);
    }

    private void FixedUpdate() //all physics adjusting code goes here
    {
        rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);
    }

    void checkLives()
    {
        //hitNoise.Play();
        playerSounds.hitSound();

        if (lives > 2) //has glasses and buff
        {
            //dont lose glasses, just a life + buff if applicable
            lives--;
        } else if (lives == 2) //has glasses but no buff
        {
            lives--;
            if (anim.GetBool("blind") == false)
                anim.SetBool("blind", true);
        } else if (lives == 1) //no glasses and no buff
        {
            lives--;
            //game over :) just reloads the scene rn
            playerSounds.reloadSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            /* If the enemy is stunned, they have no effect on Melita. 
             * Otherwise, automatically use a held powerup if it is applicable to the enemy 
             * that Melita is touching. */
            if (!other.GetComponent<Enemy>().isStunned)
            {
                // Automatically use a powerup if applicable to enemy
                // Note: the HandlePowerUp function returns true if the powerup used on them is applicable to them
                if (powerUp != PowerUp.PowerUpType.None && other.GetComponent<Enemy>().HandlePowerUp(powerUp))
                {
                    // also affects all other applicable enemies in range
                    UsePowerUp();
                }
                else
                {
                    // possible death if not enough lives
                    checkLives();
                }
            }
            
        }
        else if(other.CompareTag("Glasses")) // pick up glasses
        {
            if(anim.GetBool("blind"))
            {
                anim.SetBool("blind", false);
            }
        }
        else if(other.CompareTag("GlassesBuff"))
        {
            lives++;
            Destroy(other.gameObject);
            Debug.Log("add lives. current lives " + lives);
            //add any ui code here!
        }

    }
}
