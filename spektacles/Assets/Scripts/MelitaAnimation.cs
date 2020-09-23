using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelitaAnimation : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
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
 
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("side", true);
            anim.SetBool("back", false);
            sprite.flipX = true;
        }
        if(Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.A))
        {
            anim.SetBool("side", true);
            anim.SetBool("back", false);
            sprite.flipX = false;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            anim.SetBool("back", true);
            anim.SetBool("side", false);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S)))
        {
            anim.SetBool("back", false);
            anim.SetBool("side", false);
        }
    }
}
