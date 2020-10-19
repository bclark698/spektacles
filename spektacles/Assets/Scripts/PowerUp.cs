using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type { None, BugSpray, Helmet, EarPlugs, Garlic };
    public Type type = Type.None;
    [SerializeField] private PowerupSoundController powerUpSounds; // TODO remove serial
    private PowerUpRange powerUpRange;
    private Player player;
    private ChangeIcon indicator;

    void Awake() {
        powerUpSounds = GameObject.Find("/Unbreakable iPod/Powerup Sounds").GetComponent<PowerupSoundController>();
        powerUpRange = GameObject.FindGameObjectWithTag("PowerUp Range").GetComponent<PowerUpRange>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        indicator = GameObject.FindGameObjectWithTag("PowerUp Indicator").GetComponent<ChangeIcon>();
    }

    // for powerups that are just picked up when walked over on the ground. Move this to a different script?
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
        Debug.Log("powerUpSounds is null? "+(powerUpSounds == null));
        powerUpSounds.pickUpSound();

        // spawn cool particle effects

        // set player powerup type
        // player.heldPowerUp = type;

        // destroy the player's previous held powerup gameobject and set to new one
        powerUpRange.SetNewHeldPowerUp(gameObject);

        // show powerup in the indicator
        indicator.update(GetComponent<SpriteRenderer>().sprite);

        // turn off collider and sprite renderer for the object
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Use() {
        PlayUseSound();
        // remove powerup from indicator
        indicator.clear();

        // destroy powerup gameObject
        Destroy(gameObject);
        // player.heldPowerUp = Type.None;
    }

    void PlayUseSound() {
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
}