using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locker : MonoBehaviour
{
    // public PowerUp.Type item = PowerUp.Type.None; // powerup that can always be retrieved from this locker
    private bool playerInRange;

    // Reference to the powerUp Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject powerUpPrefab;

    public PlayerControls controls;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
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
            Debug.Log("Player picked up " + powerUpPrefab.GetComponent<PowerUp>().type.ToString() + " from a locker");

            // play a closing animation on the locker??
        }
    }
}
