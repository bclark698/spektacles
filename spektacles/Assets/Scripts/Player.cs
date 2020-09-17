using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementVelocity;
    public float moveSpeed;
    private Animator anim;
    [HideInInspector]
    public bool powerUpUsed;
    private bool hasPowerUp;
    public GameObject eyeglasses;

    //These won't actually be like this in the future - I'll just have one playerAudioSource;
    // it'll be clean, promise
    // But for now, just assist the showing of functionality
    public AudioSource tempPickupNoise;
    public AudioSource tempSprayNoise;
    public AudioSource hitNoise;

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

        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVelocity = moveInput.normalized * moveSpeed;

        //Movement Animations
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

        // Handle powerUp. Important to do .GetKeyDown(KeyCode.P) instead of .GetKey(KeyCode.P) because GetKey triggers more than once
        if (Input.GetKeyDown(KeyCode.P) && powerUp != PowerUp.PowerUpType.None)
        {
            // get all the enemies within our PowerUpRange
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(powerUpRangePos.position, powerUpRange, whatIsEnemies);

            // don't waste powerup unless enemies are in range
            if(enemiesInRange.Length > 0)
            {
                powerUpObj.GetComponent<PowerUp>().Use();
                // tempSprayNoise.Play();
                // TODO put this noise in bug spray powerup

                // have each enemy determine how to handle this powerup being used on them
                for (int i = 0; i < enemiesInRange.Length; i++)
                {
                    enemiesInRange[i].GetComponent<Enemy>().HandlePowerUp(powerUp);
                }
                // set player back to holding no powerup
                powerUp = PowerUp.PowerUpType.None;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemie" && !other.GetComponent<Enemy>().isStunned)
        {
            if(anim.GetBool("blind")==false)
                anim.SetBool("blind", true);
                hitNoise.Play();
        }
        if(other.gameObject.tag == "Glasses")
        {
            if(anim.GetBool("blind")==true)
                anim.SetBool("blind", false);
        }
    }
}
