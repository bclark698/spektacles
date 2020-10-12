using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LostNFound : MonoBehaviour
{
    public SpriteRenderer sr;
    public BoxCollider2D col;
    private bool playerInRange;
    private Player player;

    public PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.EquipOrInteract.performed += _ => Interact();
    }

    void Interact()
    {
        if (playerInRange)
        {
            if (player.HasGlasses())
            {
                // show quick popup in fungus that doesnt obscure the game saying "Nothing in here"

                // for now, just show debug message
                Debug.Log("Nothing in lost and found!");
            }
            else
            {
                player.PickUpGlasses();
            }
        }
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

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

    // unused functions currently
    public void Activate()
    {
        sr.enabled = true;
        col.enabled = true;
    }

    public void Deactivate()
    {
        sr.enabled = false;
        col.enabled = false;
    }
}
