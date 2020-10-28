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
    private int newLives; // made a new variable to make sure I didn't break anything else?? lmao
    private GameObject life2Image;

    //audio
    private PlayerSoundController playerSounds;
    private musicController musicSounds;
    private bool isWalking;

    public PlayerControls controls;
    private bool reachedEnd;
    private bool inCutscene;
    private Petrify petrify;
    private PowerUpRange powerUpRange;

    [SerializeField]
    private GameObject restart;

    // called before Start
    void Awake()
    {
        controls = new PlayerControls();
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
        petrify = GameObject.FindGameObjectWithTag("Petrify Range").GetComponent<Petrify>();
        powerUpRange = GameObject.FindGameObjectWithTag("PowerUp Range").GetComponent<PowerUpRange>();

        musicSounds = GameObject.Find("/Unbreakable iPod").GetComponent<musicController>();
        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();

        transform.GetChild(0).gameObject.SetActive(false);

        life2Image = GameObject.Find("Life 2");

        inCutscene = false;

        newLives = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(0,0);
        if(!inCutscene) {
            moveInput = controls.Gameplay.Move.ReadValue<Vector2>();
        }
        movementVelocity = moveInput.normalized * moveSpeed;
        if (movementVelocity != new Vector2(0, 0)){
            anim.SetFloat("Horizontal", moveInput.x);
            anim.SetFloat("Vertical", moveInput.y);
            anim.SetFloat("Magnitude", moveInput.magnitude);
            anim.SetBool("Moving", true);
            if (!isWalking){
                if (playerSounds != null){
                    playerSounds.FootstepLoopPlay();
                    isWalking = true;
                }
            }
        } else {

            anim.SetBool("Moving", false);
            if (playerSounds != null){
                isWalking = false;
                playerSounds.FootstepLoopStop();
            }
        }
    }

    private void FixedUpdate() //all physics adjusting code goes here
    {
        rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);
    }

    public bool HasGlasses()
    {
        return (lives >= 2);
    }

    void HandleHit() //TODO: reorganize this :)
    {
        playerSounds.HitSound();
        // game over on one hit

        newLives--;
        if (newLives == 1)
        {
            life2Image.SetActive(false);

        }
        if (newLives == 0)
        {
            playerSounds.ReloadSound();
            RestartLevel();
            lives = 0;
        }

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

    void RestartLevel()
    {
        newLives = 2;
        lives = 2;
        life2Image.SetActive(true);
        if (anim.GetBool("blind"))
        {
            anim.SetBool("blind", false);
            StopAllCoroutines();
        }
        transform.position = restart.transform.position;
    }

    public void PickUpGlasses()
    {
        if (anim.GetBool("blind"))
        {
            anim.SetBool("blind", false);
            lives = 2;
        }
        playerSounds.AcquireSound();
    }

    public void LoseGlasses()
    {
        lives = 1;
        petrify.PetrifyEnemy();
        if (anim.GetBool("blind") == false)
        {
            anim.SetBool("blind", true);
        }
    }


    // add other player-specific things if needed
    private void Interact() {
        if(reachedEnd) {
            /*the camera being turned off is now managed by fungus so we don't
            have to worry about it accidentally knocking melita out of range*/
            musicSounds.loadCustceneMusic();
            inCutscene = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        /* petrify range is a childed object of the player object with it's own collider
         * and kinematic rigidbody to allow separate trigger detection, but the triggers of
         * the parent and child object will trigger the other, so this check ignores it. */
        if(other.CompareTag("Petrify Range") || other.CompareTag("PowerUp Range")) {
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            /* If the enemy is stunned, they have no effect on Melita.
             * Otherwise, automatically use a held powerup if it is applicable to the enemy
             * that Melita is touching. */
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (!enemy.isStunned)
            {
                // Automatically use a powerup if applicable to enemy
                if(enemy.CanHandlePowerUp())
                {
                    // affects all other applicable enemies in range
                    powerUpRange.UsePowerUp();
                } else
                {
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

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("End")) {
            reachedEnd = false;
        }
    }
}
