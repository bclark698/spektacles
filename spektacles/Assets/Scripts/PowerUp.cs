using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : Ability
{
    public enum Type { None, BugSpray, Helmet, EarPlugs, Garlic };
    private PowerupSoundController powerUpSounds;

    private ChangeIcon Indicator;

    // [SerializeField] private GameObject powerUpObj; // TODO delete serialize

    void Start(){
        powerUpSounds = GameObject.Find("/Unbreakable iPod/Powerup Sounds").GetComponent<PowerupSoundController>();
        controls.Gameplay.UsePowerUp.started += _ => ButtonHeld();
        controls.Gameplay.UsePowerUp.canceled += _ => ButtonRelease();
    }

    void ButtonRelease() {
        buttonHeld = false;
        UsePowerUp();
    }

    /* 
    // for powerups that are just picked up when walked over on the ground. Move this to a different script?
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUp();
        }
    }*/

    public void PickUp(Type type)
    {
        // play a pick up sound effect
        powerUpSounds.pickUpSound();

        // spawn cool particle effects

        // set player powerup type
        player.heldPowerUp = type;

        // destroy the player's previous held powerup gameobject and set to new one
        // Destroy(powerUpObj);
        // powerUpObj = gameObject;

        // show powerup in the indicator
        Indicator = GameObject.FindGameObjectWithTag("PowerUp Indicator").GetComponent<ChangeIcon>(); //TODO move to Start()?
        Indicator.update(GetComponent<SpriteRenderer>().sprite);

        // turn off collider and sprite renderer for the object
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    /* Handle powerUp. A held powerUp still gets used (wasted) even if no enemies are
     * in range to let player try out using powerups. */
    public void UsePowerUp()
    {
        Type heldPowerUp = player.heldPowerUp;
        if (heldPowerUp != Type.None)
        {
            // get all the enemies within our PowerUpRange
            Collider2D[] enemiesInRange = GetEnemiesInRange();

            // temporarily keep track of the held powerup item because .Use() sets powerUp to None.
            Type temp = heldPowerUp; //TODO can this be removed?

            // powerUpObj.Use();
            PlayUseSound(heldPowerUp);
            // remove powerup from indicator
            Indicator.clear();

            // destroy powerup gameObject
            // Destroy(gameObject);
            player.heldPowerUp = Type.None;

            // have each enemy determine how to handle this powerup being used on them
            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().HandlePowerUp(temp);
                enemiesInRange[i].GetComponent<Enemy>().OutlineOff();
            }

        }
    }

    void PlayUseSound(Type type) {
        // play a use sound effect (specialized for each powerup)
        // specialized powerup particle effect TODO
        switch (type) {
            case Type.BugSpray:
                powerUpSounds.bugSpraySound();
                break;

            case Type.Helmet:
                powerUpSounds.helmetBlockSound();
                break;

            case Type.EarPlugs:
                //powerUpSounds.earbudsBlockSoundStart(); //(This would be a sound that signifies earbuds breaking.. so dunno)
                break;

            case Type.Garlic:
                powerUpSounds.garlicBlockSound();
                break;
        }
        /*  case Type.Dash:
                powerUpSounds.zoomSound();
                break;*/
    }

    /* For testing purposes, this draws red line around the player's power up range.
     * This has no effect during gameplay, so we can leave this in. */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
