using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sirens : MonoBehaviour
{
    private Rigidbody2D playerRB; //var for player rigidbody
    private bool inRange = false;
    
    [SerializeField] private float gravitationalForce = 5; //adjust gravity


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>(); //grab player RB
    }

    private void FixedUpdate()
    {
        if (inRange)
        {
            // make a vector 2 of wherever the player is
            var directionOfPlayer = ((Vector2)transform.position - playerRB.position).normalized;

            // adds force to the player to pull them towards siren
            playerRB.AddForce(directionOfPlayer * gravitationalForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //once u enter the range
    {
        if(collision.CompareTag("Player"))
        {
            inRange = true; //start pulling player
        }

    }

    private void OnTriggerExit2D(Collider2D collision) //once u leave the range
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false; //stop pulling player
        }
    }


}

