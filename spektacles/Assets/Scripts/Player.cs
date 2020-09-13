using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movementVelocity;
    public float moveSpeed;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVelocity = moveInput.normalized * moveSpeed;
        anim.SetBool("walking", true);
    }

    private void FixedUpdate() //all physics adjusting code goes here
    {
        rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);
    }

}
