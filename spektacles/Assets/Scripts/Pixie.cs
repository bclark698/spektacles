using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixie : MonoBehaviour
{
    ////////////
    // STATES //
    ////////////
    private enum State
    {
        Waiting,        // waiting to see enemy
        ChaseTarget,    // chasing enemy
        ReturnToStart,  // returning to starting point if loses vision of player
    }


    ///////////////////////
    // PRIVATE VARIABLES //
    ///////////////////////
    private State state;            // current state of enemy (chasing, roaming, etc)
    private Vector2 startingPos;    // pixies' starting position
    private RaycastHit2D hitInfo;   // ray that searches for player
    private RaycastHit2D wallHit;   // ray that looks for walls / blocking objects
    private GameObject playerObj;   // player object (melita)
    private bool playerHit;         // true if player has been hit, false if not
    private bool atHome;            // true if pixies are at starting position


    ///////////////////////////////////////
    // PUBLIC VARIABLES / UNITY EDITABLE //
    ///////////////////////////////////////
    public float rotationSpeed; // speed at which pixies spin
    public float fovDistance;   // distance away from pixies that they can see
    public float moveSpeed;     // movement speed of pixies
    public AudioSource giggle1;
    public AudioSource giggle2;


    ////////////////////
    // MAIN FUNCTIONS //
    ////////////////////

    // Awake is called before Start
    private void Awake()
    {
        state = State.Waiting; // pixie's begin roaming upon level start
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false; // stops ray from detecting pixies own collider
        startingPos = transform.position; // gets pixie's starting position
        atHome = true;
        playerObj = GameObject.FindGameObjectWithTag("Player"); // create player object

    }

    // Update is called once per frame
    void Update()
    {

        // casts ray starting at transform.pos; casts in direction transform.right; length of ray = fovDistance
        hitInfo = Physics2D.Raycast(transform.position, transform.right, fovDistance);

        switch(state)
        {
            
            // CASE 1
            // Rotate. If player found, change state to chasing target.
            default:
            case State.Waiting:
                lookForPlayer();
                break;
            
            // CASE 2
            // Chase target until line of sight is broken or power-up is used.
            case State.ChaseTarget:
                chaseTarget();
                break;

            // CASE 3
            // Return to start if line of sight is broken or power-up is used.
            case State.ReturnToStart:
                StartCoroutine(returnHome());           
                break;
        }

    }


    //////////////////////
    // HELPER FUNCTIONS //
    //////////////////////

    // check if player is in line of sight
    private bool canSeePlayer()
    {
        bool collisionCheck = false;
        if (hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            if (hitInfo.collider.CompareTag("Player"))
            {
                collisionCheck = true;
                atHome = false;
            }
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * fovDistance, Color.green);
            collisionCheck = false;
        }
        return collisionCheck;
    }

    // check if pixie loses sight of player
    private bool cantSeePlayer()
    {
        bool collisionCheck = false;
        if (wallHit.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            if (hitInfo.collider.CompareTag("Wall")) //FIX THIS; needs to check for any non player objects (walls, tables, doors)
            {
                collisionCheck = true;
            }
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * fovDistance, Color.green);
            collisionCheck = false;
        }
        return collisionCheck;
    }

    // check if pixies touches player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHit = true;
            giggle2.Play();
        }
    }


    // searches for player
    private void lookForPlayer()
    {
        if (!canSeePlayer())
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else
        {
            giggle1.Play();
            state = State.ChaseTarget;
            //Debug.Log("Chasing Target");
        }
    }

    // causes pixies to chase player
    private void chaseTarget()
    {
        wallHit = Physics2D.Raycast(transform.position, transform.right, fovDistance);
        // move towards the player using Vector2.MoveTowards(fromPosition, toPosition, speed);
        if (!playerHit)
        {
            // causes pixies to continously look towards player
            Vector3 direction = playerObj.transform.position - transform.position; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(2, 2, angle);

            transform.position = Vector2.MoveTowards(transform.position, playerObj.GetComponent<Transform>().position, moveSpeed * Time.deltaTime);
            if (cantSeePlayer())
            {
                state = State.ReturnToStart;
            }
        }
        else
        {
            state = State.ReturnToStart;
            playerHit = false;
        }
        //if(nonPlayerSightCheck()) // CURRENTLY DOESN'T WORK
        //{
        //    state = State.ReturnToStart;
        //    giggle2.Play();
        //    Debug.Log("ha ha ");
        //}
        //if(powerUpUsed)
        //{
        //    state = State.ReturnToStart;
        //}
    }

    // sends pixies back to starting position
    IEnumerator returnHome()
    {
        float reachedPosDist = 1f;
        while (Vector2.Distance(transform.position, startingPos) > reachedPosDist)
        {
            transform.position = Vector2.MoveTowards(transform.position, startingPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        atHome = true;
        state = State.Waiting;
    }

}
