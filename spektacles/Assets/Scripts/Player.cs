using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Fungus;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementVelocity;
    public float moveSpeed = 20f;
    private Animator anim;
    private int lives = 2; //one for w/ glasses, one for without
    private Follow cameraF;
    public BoxCollider2D irving;

    //dash stuff
    public float dashSpeed;
    public float startDashTime;
    private float dashTime;

    //audio
    private PlayerSoundController playerSounds;
    private PowerupSoundController powerupSounds;
    private musicController musicSounds;


    // powerUp variables
    public PowerUp.PowerUpType powerUp = PowerUp.PowerUpType.None; // TODO make this a private serialized field?
    [SerializeField]
    public Transform powerUpRangePos;
    [SerializeField]
    public LayerMask whatIsEnemies;
    [SerializeField]
    public float powerUpRange = 10f;
    public GameObject powerUpObj;

    public Text powerUpText;
    public GameObject sprayEffect;

    public PlayerControls controls;
    private bool reachedEnd;
    private bool inCutscene;


    [SerializeField] bool showMovementIndicator = false; // should set to true in inspector for melita in the first home scene

    // called before Start
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.UsePowerUp.performed += _ => UsePowerUp();
        controls.Gameplay.Pause.performed += _ => Pause();
        controls.Gameplay.EquipOrInteract.performed += _ => Interact();
        //controls.Gameplay.Move.performed += context => Move(context.ReadValue<Vector2>());
    }

    // TODO make a separate script for pause
    void Pause()
    {
        Debug.Log("pause button pressed");
    }

    // Called when the Player object is enabled
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cameraF = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Follow>();

        musicSounds = GameObject.Find("/Unbreakable iPod").GetComponent<musicController>();
        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();
        powerupSounds = GameObject.Find("/Unbreakable iPod/Powerup Sounds").GetComponent<PowerupSoundController>();

        transform.GetChild(0).gameObject.SetActive(false);

        // TODO delete later when implement outline enemies
        // changes power up range indicator to be proper size
        powerUpRangePos.localScale = new Vector3(2*powerUpRange, 2*powerUpRange, 0);
        // make sure it isn't visible at the start of the game
        GameObject.FindGameObjectWithTag("PowerUp Range").GetComponent<SpriteRenderer>().enabled = false;

        dashTime = startDashTime;
        // if home scene
        if(showMovementIndicator) {
            StartCoroutine(GetComponent<ControlsIndicator>().ShowForDuration(ControlsIndicator.Icon.Movement, 2f));
        }
        inCutscene = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!inCutscene) {
            Vector2 moveInput = controls.Gameplay.Move.ReadValue<Vector2>();
            movementVelocity = moveInput.normalized * moveSpeed;
            if (movementVelocity != new Vector2(0, 0)){
                anim.SetFloat("Horizontal", moveInput.x);
                anim.SetFloat("Vertical", moveInput.y);
                anim.SetFloat("Magnitude", moveInput.magnitude);
                anim.SetBool("Moving", true);
            } else {
                anim.SetBool("Moving", false);
            }
        } else {
            anim.SetBool("Moving", false);
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
            Collider2D[] enemiesInRange = GetEnemiesInRange();

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

    Collider2D[] GetEnemiesInRange()
    {
        // get all the enemies within our PowerUpRange
        return Physics2D.OverlapCircleAll(powerUpRangePos.position, powerUpRange, whatIsEnemies);
    }


    /* For testing purposes, this draws red line around the player's power up range.
     * This has no effect during gameplay, so we can leave this in. */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(powerUpRangePos.position, powerUpRange);
    }

    public bool HasGlasses()
    {
        return (lives >= 2);
    }

    void HandleHit()
    {
        playerSounds.HitSound();
        // game over on one hit
        playerSounds.ReloadSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        lives = 0;
        //TODO allow for glasses with buff?

        /*
        if (lives == 2) //has glasses but no buff
        {
            LoseGlasses();
        } else if (lives == 1) //no glasses and no buff
        {
            //game over :) just reloads the scene rn
            playerSounds.ReloadSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        lives--;*/
    }

    public void PickUpGlasses()
    {
        if (anim.GetBool("blind"))
        {
            anim.SetBool("blind", false);
            lives = 2;
        }
        playerSounds.AcquireSound();
        irving.isTrigger = true;
    }

    public void LoseGlasses()
    {
        lives = 1;
        GetComponent<Petrify>().PetrifyEnemy();
        if (anim.GetBool("blind") == false)
        {
            anim.SetBool("blind", true);
        }
        irving.isTrigger = false; //turn irving off
    }


    // add other player-specific things if needed
    private void Interact() {
        if(reachedEnd) {
            //turn everything off so the player cant lose when they talk to irving
            //important!!!! must turn off the WHOLE OBJECT bc pixies will not stop otherwise
            //irving is not able to handle 'complex' collisions so thats on the player
            /* don't turn off the player when turning off camera follow because it will 
             * say the player is no longer in range */
            cameraF.stopFollow(false);
            musicSounds.loadCustceneMusic();
            inCutscene = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            /* If the enemy is stunned, they have no effect on Melita.
             * Otherwise, automatically use a held powerup if it is applicable to the enemy
             * that Melita is touching. */
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (!enemy.isStunned)
            {
                // Automatically use a powerup if applicable to enemy
                // Note: the HandlePowerUp function returns true if the powerup used on them is applicable to them
                if (powerUp != PowerUp.PowerUpType.None)
                {
                    if(enemy.HandlePowerUp(powerUp))
                    {
                        // also affects all other applicable enemies in range
                        UsePowerUp();
                    } else
                    {
                        HandleHit();
                    }
                }
                else
                {
                    // possible death if not enough lives
                    HandleHit();
                }
            }

        }
        else if (other.CompareTag("Glasses") || other.CompareTag("LostNFound"))
        {
            PickUpGlasses();
        }
        else if (other.CompareTag("GlassesBuff"))
        {
            lives++;
            Destroy(other.gameObject);
            Debug.Log("add lives. current lives " + lives);
            playerSounds.AcquireSound();
            //add any ui code here!
        }
        else if(other.CompareTag("End") && HasGlasses())
        {
            reachedEnd = true;
        }
        else if(other.CompareTag("StartNextScene"))
        {
            reachedEnd = false;
            SceneManager.LoadScene(1);
        //  musicSounds.loadCustceneMusic();
        }

    }


     // makes melita zoom zoom
     public void Dash()
     {
        /*
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
            }*/

     }
}
