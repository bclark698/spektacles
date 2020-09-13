using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixies : MonoBehaviour
{
    public float moveSpeed;
    private GameObject playerObj;
    private Transform player;
    public float agroDistance; // how close to the player to start chasing
    // public float stoppingDistance; // how close to the player to stop chasing to prevent the sprites from overlapping too much

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // if player in range of pixie, then continue moving to chase player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (!playerObj.GetComponent<Player>().inSafeZone && distanceToPlayer < agroDistance)
        {
            // move towards the player using Vector2.MoveTowards(fromPosition, toPosition, speed);
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        
    }
}
