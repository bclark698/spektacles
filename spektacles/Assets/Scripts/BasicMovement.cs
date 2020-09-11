using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKey) //if nothing is pressed, don't move
        {
            rb.velocity = new Vector2(0, 0);
            anim.Play("Melita_Idle");
        }
        //moving up
        else if (Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.W)))
        {
            rb.velocity = new Vector2(0, speed);
            anim.Play("Melita_Walk");
        }
        //moving down
        else if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S)))
        {
            rb.velocity = new Vector2(0, -speed);
            anim.Play("Melita_Walk");
        }
        //moving left
        else if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)))
        {
            rb.velocity = new Vector2(-speed, 0);
            anim.Play("Melita_Walk");
        }
        //moving right
        else if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D)))
        {
            rb.velocity = new Vector2(speed, 0);
            anim.Play("Melita_Walk");
        }
    }
}
