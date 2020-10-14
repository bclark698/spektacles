using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locker : MonoBehaviour
{
    [SerializeField]
    public PowerUp.PowerUpType item = PowerUp.PowerUpType.None; // powerup that can always be retrieved from this locker
    private bool playerInRange;

    // Reference to the powerUp Prefab. Drag a Prefab into this field in the Inspector.
    [SerializeField]
    public GameObject powerUpPrefab;

    private PowerupSoundController powerupSounds;

    public PlayerControls controls;

    private ShowInteractIndicator interactIndicator;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.EquipOrInteract.performed += _ => Open();
    }

    // Called when the Player object is enabled
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start(){
        powerupSounds = GameObject.Find("/Unbreakable iPod/Powerup Sounds").GetComponent<PowerupSoundController>();
        interactIndicator = GetComponent<ShowInteractIndicator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            interactIndicator.Show(ShowInteractIndicator.Icon.Interact);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
            interactIndicator.Hide();
        }
    }

    // This script will simply instantiate the Prefab whenever the locker is opened.
    public void Open()
    {
        if(playerInRange && powerUpPrefab != null)
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
