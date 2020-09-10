using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public int speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKey)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(0, speed);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(0, -speed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(speed, 0);
        }
    }
}
