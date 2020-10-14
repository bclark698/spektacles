using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { None, BugSpray, Helmet, EarPlugs, Garlic, Dash };
    public PowerUpType powerUpName;

    protected Player p;

    [SerializeField]
    public float duration = 3f; // how long the powerup is in use for in seconds I think

    private PowerupSoundController powerupSounds;

    private ChangeIcon Indicator;

    public SpriteRenderer powerUpRange; // used to display range circle (TODO replace with enemy highlighting later)

    void Start(){
        powerupSounds = GameObject.Find("/Unbreakable iPod/Powerup Sounds").GetComponent<PowerupSoundController>();
    }

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

        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // set player powerup type
        p.powerUp = powerUpName;

        // destroy the player's previous held powerup gameobject and set to new one
        Destroy(p.powerUpObj);
        p.powerUpObj = gameObject;

        // show powerup in the indicator
        Indicator = GameObject.FindGameObjectWithTag("PowerUp Indicator").GetComponent<ChangeIcon>();
        Indicator.update(GetComponent<SpriteRenderer>().sprite);

        // turn off collider and sprite renderer for the object
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // show powerup range around player
        powerUpRange = GameObject.FindGameObjectWithTag("PowerUp Range").GetComponent<SpriteRenderer>();
        powerUpRange.enabled = true;
}

    // default Use function unless overridden in child classes
    public virtual void Use()
    {
        // play a use sound effect (specialized for each powerup)
        // specialized powerup particle effect TODO
        switch (powerUpName){
        case PowerUpType.BugSpray:
        powerupSounds.bugSpraySound();
        break;

        case PowerUpType.Helmet:
        powerupSounds.helmetBlockSound();
        break;

        case PowerUpType.EarPlugs:
        //powerupSounds.earbudsBlockSoundStart(); //(This would be a sound that signifies earbuds breaking.. so dunno)
        break;

        case PowerUpType.Garlic:
        powerupSounds.garlicBlockSound();
        break;

        case PowerUpType.Dash:
        powerupSounds.zoomSound();
        break;
        }


        // remove powerup from indicator
        Indicator.clear();

        // destroy powerup gameObject
        Destroy(gameObject);
        p.powerUp = PowerUpType.None;
        p.powerUpText.text = "Held Powerup: " + p.powerUp;

        // unshow powerup range indicator around player
        powerUpRange.enabled = false;
    }
}
