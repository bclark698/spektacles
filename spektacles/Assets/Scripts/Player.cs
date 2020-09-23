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
    //dash stuff
    public float dashSpeed;
    public float startDashTime;
    private float dashTime;
    private int direction;

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

        dashTime = startDashTime;
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

            // temporarily keep track of the held powerup item because .Use() sets powerUp to None.
            PowerUp.PowerUpType temp = powerUp;

            if(powerUpObj != null)
            {
                powerUpObj.GetComponent<PowerUp>().Use();

                // tempSprayNoise.Play();
                // TODO put this sound effect in bug spray powerup/pixie code

                // have each enemy determine how to handle this powerup being used on them
                for (int i = 0; i < enemiesInRange.Length; i++)
                {
                    enemiesInRange[i].GetComponent<Enemy>().HandlePowerUp(temp);
                }
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
        if (other.CompareTag("Enemy"))
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

    // makes melita zoom zoom
    public void dashyShit()
    {
        if (direction == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = 2;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = 3;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = 4;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (direction == 1)
                {
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (direction == 2)
                {
                    rb.velocity = Vector2.right * dashSpeed;
                }
                else if (direction == 3)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
                else if (direction == 4)
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }
            }
        }
    }
}
