using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementVelocity;
    public float moveSpeed;
    private Animator anim;
    public bool inSafeZone;
    public GameObject eyeglasses;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inSafeZone = false;
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
        }
        if(other.gameObject.tag == "Glasses")
        {
            if(anim.GetBool("blind")==true)
                anim.SetBool("blind", false);
        }
    }
}
