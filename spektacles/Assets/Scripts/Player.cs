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
    private GameObject life2Image;
    private GameObject life3Image;
    private Sprite lifeSpritePink;
    private Sprite lifeSpriteGrey;
    private Sprite lifeSpriteGold;
    private Color gold = new Color(1f, .84f, 0f);

    Vector2 moveDirection;
    Collider2D goldHeart;

    //audio
    private PlayerSoundController playerSounds;
    private musicController musicSounds;
    private bool isWalking;

    public PlayerControls controls;
    private bool reachedEnd;
    public static bool inCutscene;
    private Petrify petrify;
    private PowerUpRange powerUpRange;

    private DamageCooldown damageCooldown;
    [HideInInspector] public bool invincible;


    [SerializeField]
    private GameObject restart = null;
    public static bool allowMovement = true;
    public static bool allowInteract = true;

    // called before Start
    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.EquipOrInteract.performed += _ => Interact();
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

        GameObject petrifyObject = GameObject.FindGameObjectWithTag("Petrify Range");
        if(petrifyObject) {
            petrify = petrifyObject.GetComponent<Petrify>();
        }
        GameObject powerUpObject = GameObject.FindGameObjectWithTag("PowerUp Range");
        if(powerUpObject) {
            powerUpRange = powerUpObject.GetComponent<PowerUpRange>();
        }
        damageCooldown = GetComponent<DamageCooldown>();

        musicSounds = GameObject.Find("/Unbreakable iPod").GetComponent<musicController>();
        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();

        transform.GetChild(0).gameObject.SetActive(false);

        life2Image = GameObject.Find("Life 2");
        life3Image = GameObject.Find("Life 3");
        if(life3Image != null){
            life3Image.SetActive(false);
          }
        if(GameObject.Find("Life Indication Sprites") != null){
        lifeSpritePink = GameObject.Find("Pink Heart").GetComponent<Image>().sprite;
        lifeSpriteGrey = GameObject.Find("Grey Heart").GetComponent<Image>().sprite;
        lifeSpriteGold = GameObject.Find("Gold Heart").GetComponent<Image>().sprite;
        }

        inCutscene = false;
        PauseMenu.allowPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(0,0);
        if(allowMovement && !inCutscene) {
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

    void HandleHit()
    {
        if (!invincible) //TODO: move this check to onTriggerEnter?
        {
            playerSounds.HitSound();
            Debug.Log("lives before minus:" + lives);
            lives--;
            Debug.Log("lives after minus:" + lives);
            if (lives == 2)
            {
              //  life3Image.SetActive(false);
              life3Image.GetComponent<Image>().sprite = lifeSpriteGrey;
                damageCooldown.StartTimer();
            }
            if (lives == 1)
            {
              //  life2Image.SetActive(false);
              life2Image.GetComponent<Image>().sprite = lifeSpriteGrey;
                damageCooldown.StartTimer();
            }
            else if (lives <= 0)
            {
                RestartLevel();
                goldHeart.gameObject.SetActive(true);
            }
        }

        else if(invincible)
        {
            Debug.Log("invincible, no hit");
        }
    }

    void RestartLevel()
    {
        playerSounds.ReloadSound();
        lives = 2;
        life2Image.GetComponent<Image>().sprite = lifeSpritePink;
        life3Image.GetComponent<Image>().sprite = lifeSpriteGold;
        life3Image.SetActive(false);
        if (anim.GetBool("blind"))
        {
            anim.SetBool("blind", false);
            var blurBox = Camera.main.GetComponent<BoxBlur>();
            blurBox.enabled = false;
            StopAllCoroutines();
        }
        Debug.Log("Restart. lives:" + lives);
        transform.position = restart.transform.position;
    }

    public void PickUpGlasses()
    {
        if (anim.GetBool("blind"))
        {
            anim.SetBool("blind", false);
            //lives = 2; //Turned this off cause it was causing a bug w/ the buff
        }
    }

    public void LoseGlasses()
    {
        petrify.PetrifyEnemy();
        if (anim.GetBool("blind") == false)
        {
            anim.SetBool("blind", true);
        }
    }


    // add other player-specific things if needed
    private void Interact() {
        if(allowInteract && reachedEnd) {
            /*the camera being turned off is now managed by fungus so we don't
            have to worry about it accidentally knocking melita out of range*/
            musicSounds.loadCustceneMusic();
            inCutscene = true;
            PauseMenu.allowPause = false;
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
                    moveDirection = rb.transform.position - other.transform.position;
                    rb.AddForce(moveDirection.normalized * 25000f);
                }
            }

        }
        else if (other.CompareTag("Glasses")) //TODO: do we need this?
        {
            PickUpGlasses();
        }
        else if (other.CompareTag("GlassesBuff"))
        {
            lives = 3;
            life2Image.GetComponent<Image>().sprite = lifeSpritePink;
            life3Image.SetActive(true);
            life3Image.GetComponent<Image>().sprite = lifeSpriteGold;


            //Destroy(other.gameObject);
            goldHeart = other;
            other.gameObject.SetActive(false);
            Debug.Log("add lives. current lives " + lives);
            playerSounds.AcquireSound();
            //add any ui code here!
        }
        else if(other.CompareTag("End") && HasGlasses())
        {
            reachedEnd = true;
        }

    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("End")) {
            reachedEnd = false;
        }
    }
}
