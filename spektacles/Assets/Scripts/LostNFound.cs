using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostNFound : MonoBehaviour
{
    public SpriteRenderer sr;
    public BoxCollider2D col;
    private bool playerInRange;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if(player.HasGlasses())
            {
                // show quick popup in fungus that doesnt obscure the game saying "Nothing in here"

                // for now, just show debug message
                Debug.Log("Nothing in lost and found!");
            } else
            {
                player.PickUpGlasses();
            }
        }
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
