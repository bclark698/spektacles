using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sirens : Enemy
{
    private Rigidbody2D playerRB; //var for player rigidbody
    private bool inRange = false; //if player is in range
    
    [SerializeField] private float gravitationalForce = 5; //adjust gravity
    private PowerUp.PowerUpType sirenPowerUp = PowerUp.PowerUpType.EarPlugs;

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
        if(collision.CompareTag("Player") && !isStunned)
        {
            inRange = true; //start pulling player
        }

    }

    private void OnTriggerExit2D(Collider2D collision) //once u leave the range
    {
        if (collision.CompareTag("Player") && !isStunned)
        {
            inRange = false; //stop pulling player
        }
    }


    public override bool HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("siren handling powerup " + powerUp);
        if (powerUp == sirenPowerUp)
        {
            StartCoroutine(HandleStun());
            return true;
        }
        return false;
    }

    public override IEnumerator HandleStun()
    {
        isStunned = true; //mark as stunned
        inRange = false; //mark player out of range (no continuing to pull)
        gameObject.GetComponent<CircleCollider2D>().enabled = false; //turn off range

        // wait for 5 seconds - long enough to get across range
        yield return new WaitForSeconds(5f);

        gameObject.GetComponent<CircleCollider2D>().enabled = true; //turn on range
        isStunned = false; //no longer stunned 
    }

}

