using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { None, BugSpray, Helmet, EarPlugs, Garlic, Dash };
    public PowerUpType powerUpName;

    private Player p;

    // currently UNUSED
    [SerializeField]
    public float duration = 3f; // how long the powerup is in use for in seconds I think

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        // play a pick up sound effect
        //tempPickupNoise.Play();

        // spawn cool particle effects

        // set player powerup variables
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        p.powerUp = powerUpName;
        p.powerUpObj = gameObject; // TODO do we need to destroy the player's previous held powerup gameobject?
        Debug.Log("Player picked up " + p.powerUp);

        // turn off collider and sprite renderer for the object
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Use()
    {
        // play a use sound effect (specialized for each powerup?)

        // specialized powerup particle effect?

        // destroy powerup gameObject
        Destroy(gameObject);
        p.powerUp = PowerUpType.None;
    }


    public class Dash : PowerUp
    {
        // 3 uses, when at 0 (aka last use) destroy
        private int numUses = 2;
        public void Use()
        {
            if(numUses == 0)
            {
                Destroy(gameObject);
                numUses = 2;
            }
            else
            {
                numUses--;
            }
        }
    }
}


