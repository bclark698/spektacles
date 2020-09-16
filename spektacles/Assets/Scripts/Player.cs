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

        if(hasPowerUp && Input.GetKeyDown(KeyCode.P)) //TODO: this is not finalized
        {
            powerUpUsed = true; //tho it will only work on pixies 4 right now
            hasPowerUp = false;
            tempSprayNoise.Play();
            transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("use powerup");
        }
    }

    private void FixedUpdate() //all physics adjusting code goes here
    {
        rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemie")
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
        if(other.CompareTag("Powerup")) //TODO: delete this && make actual powerup script
        {
            Debug.Log("has powerup");
            transform.GetChild(0).gameObject.SetActive(true);
            tempPickupNoise.Play();
            hasPowerUp = true;
        }

    }
}
