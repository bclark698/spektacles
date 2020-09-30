using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementVelocity;
    public float moveSpeed;
    private Animator anim;
    public GameObject eyeglasses;
    private int lives = 2; //one for w/ glasses, one for without
    private cameraFollow cameraF;
    public CircleCollider2D irving;

    //dash stuff
    public float dashSpeed;
    public float startDashTime;
    private float dashTime;

    //audio
    private PlayerSoundController playerSounds;
    private PowerupSoundController powerupSounds;
    private musicController musicSounds;


    // powerUp variables
    public PowerUp.PowerUpType powerUp = PowerUp.PowerUpType.None;
    public Transform powerUpRangePos;
    public LayerMask whatIsEnemies;
    public float powerUpRange;
    public GameObject powerUpObj; //TODO make private
    public Text powerUpText;
    public GameObject sprayEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cameraF = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraFollow>();
        //irving = GameObject.FindGameObjectWithTag("End").GetComponent<CircleCollider2D>();


        musicSounds = GameObject.Find("/Unbreakable iPod").GetComponent<musicController>();
        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();
        powerupSounds = GameObject.Find("/Unbreakable iPod/Powerup Sounds").GetComponent<PowerupSoundController>();

        transform.GetChild(0).gameObject.SetActive(false);

        dashTime = startDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVelocity = moveInput.normalized * moveSpeed;

        anim.SetFloat("Horizontal", moveInput.x);
        anim.SetFloat("Vertical", moveInput.y);
        anim.SetFloat("Magnitude", moveInput.magnitude);

        /* Important to use.GetKeyDown(KeyCode.P) instead of.GetKey(KeyCode.P) because
         * GetKey triggers more than once */
        if (Input.GetKeyDown(KeyCode.P))
        {
            UsePowerUp();
            if(powerUp == PowerUp.PowerUpType.BugSpray)
            {
                sprayEffect.SetActive(true);
                //Destroy(sprayEffect, 100f);
            }
        }
    }

    private void FixedUpdate() //all physics adjusting code goes here
    {
        rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);
    }

    /* Handle powerUp. A held powerUp still gets used (wasted) even if no enemies are
     * in range to let player try out using powerups. */
    void UsePowerUp()
    {
        if (powerUp != PowerUp.PowerUpType.None)
        {
            // get all the enemies within our PowerUpRange
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(powerUpRangePos.position, powerUpRange, whatIsEnemies);

            // temporarily keep track of the held powerup item because .Use() sets powerUp to None.
            PowerUp.PowerUpType temp = powerUp;

            //TODO this if might not be needed
            if (powerUpObj != null)
            {
                powerUpObj.GetComponent<PowerUp>().Use();

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


    public void checkLives()
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
            {
                anim.SetBool("blind", true);
                Camera mainCamera = Camera.main;
                mainCamera.GetComponent<BoxBlur>().enabled = true;
            }
            irving.isTrigger = false; //turn irving off

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
        else if (other.CompareTag("Glasses")) // pick up glasses
        {
            if (anim.GetBool("blind"))
            {
                anim.SetBool("blind", false);
                Camera mainCamera = Camera.main;
                mainCamera.GetComponent<BoxBlur>().enabled = false;
                lives = 2;
            }
            playerSounds.aquireSound();
            irving.isTrigger = true;
        }
        else if (other.CompareTag("GlassesBuff"))
        {
            lives++;
            Destroy(other.gameObject);
            Debug.Log("add lives. current lives " + lives);
            playerSounds.aquireSound();
            //add any ui code here!
        }
        else if(other.CompareTag("End"))
        {
            //turn everything off so the player cant lose when they talk to irving
            //important!!!! must turn off the WHOLE OBJECT bc pixies will not stop otherwise
            //irving is not able to handle 'complex' collisions so thats on the player
            cameraF.stopFollow(true); //camera follow turned off separately
            musicSounds.loadCustceneMusic();
        }
        else if(other.CompareTag("StartNextScene"))
        {
          SceneManager.LoadScene(1);
          musicSounds.LoadHallScene();
        }

    }


    // makes melita zoom zoom
    public void Dash()
    {
        Debug.Log("zoom?");
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (dashTime <= 0)
        {
            dashTime = startDashTime;
            movementVelocity = Vector2.zero;
        }
        else
        {
            dashTime -= Time.deltaTime;
            movementVelocity = direction.normalized * dashSpeed;
            Debug.Log("dashSpeed = " + dashSpeed);
            Debug.Log("Movement Velocity = " + movementVelocity);
            FixedUpdate();
        }
    }
}
