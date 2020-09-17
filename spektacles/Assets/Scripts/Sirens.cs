using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sirens : MonoBehaviour
{
    private Rigidbody2D playerRB; //var for player rigidbody
    private bool inRange = false;
    private PowerUp.PowerUpType sirenPowerUp = PowerUp.PowerUpType.EarPlugs;

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

    public override void HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("siren handling powerup " + powerUp);
        if (powerUp == sirenPowerUp)
        {
            //stop pulling
            inRange = false;
        }
        /* //TODO stun not fully implemented yet
         else if(powerUp == PowerUp.Stun) {
            StartCoroutine(HandleStun());
         }
          */
    }

    public override IEnumerator HandleStun()
    {
        // stop movement for a few seconds
        isStunned = true;

        // wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        isStunned = false;
    }

}

