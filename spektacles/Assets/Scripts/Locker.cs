using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    [SerializeField]
    private PowerUp.PowerUpType item = PowerUp.PowerUpType.None; // powerup that can always be retrieved from this locker
    private bool playerInRange; //

    // Reference to the powerUp Prefab. Drag a Prefab into this field in the Inspector.
    [SerializeField]
    public GameObject powerUpPrefab;

    private PowerupSoundController powerupSounds;

    private void Start(){
        powerupSounds = GameObject.Find("/Unbreakable iPod/Powerup Sounds").GetComponent<PowerupSoundController>();
    }

    private void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Open();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // This script will simply instantiate the Prefab whenever the locker is opened.
    public void Open()
    {
        if(powerUpPrefab != null)
        {
            // play an open animation on the locker?? to reveal the powerup inside?
            // TODO possibly

            GameObject newPowerUp = Instantiate(powerUpPrefab);

            newPowerUp.GetComponent<PowerUp>().PickUp();
            Debug.Log("Player picked up " + powerUpPrefab.GetComponent<PowerUp>().powerUpName + " from a locker");

            powerupSounds.pickUpSound();

            // play a closing animation on the locker??
        }
    }
}
