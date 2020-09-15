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
        Search,         // search for enemy after losing
        ReturnToStart,  // returning to starting point if loses vision of player
    }


    ///////////////////////
    // PRIVATE VARIABLES //
    ///////////////////////
    private State state;            // current state of enemy (chasing, roaming, etc)
    private Vector2 startingPos;    // pixies' starting position
    private RaycastHit2D hitInfo;   // ray that searches for player
    private RaycastHit2D wallHit;   // ray that looks for walls / blocking objects
    private RaycastHit2D searchHit; // ray that searches for player after losing
    private GameObject playerObj;   // player object (melita)
    private Vector2 lastSeenPos;    // players last seen position
    private bool playerHit;         // true if player has been hit, false if not
    private bool atHome;            // true if pixies are at starting position
    //private SpriteRenderer spriteRenderer; // used to flip sprite depending on movement direction


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
        //this.spriteRenderer = this.GetComponent<SpriteRenderer>(); // get spriteRenderer component
    }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false; // stops ray from detecting pixies own collider
        startingPos = transform.position; // gets pixie's starting position
        atHome = true;
        playerObj = GameObject.FindGameObjectWithTag("Player"); // create player object
        lastSeenPos = playerObj.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //this.spriteRenderer.flipX = playerObj.transform.position.x < this.transform.position.x;
        // casts ray starting at transform.pos; casts in direction transform.right; length of ray = fovDistance
        hitInfo = Physics2D.Raycast(transform.position, transform.right, fovDistance);

        switch (state)
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

            // CASE 4
            // Search for player after losing vision
            case State.Search:
                StartCoroutine(searchForPlayer());
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
            if (wallHit.collider.CompareTag("Wall")) //FIX THIS; needs to check for any non player objects (walls, tables, doors)
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
        if (collision.tag == "Player")
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
                lastSeenPos = playerObj.transform.position;
                state = State.Search;
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
            transform.position = Vector2.MoveTowards(transform.position, startingPos, (moveSpeed * Time.deltaTime) / 50);
            yield return null;
        }
        atHome = true;
        state = State.Waiting;
    }


    // pixie searches for player after losing sight
    IEnumerator searchForPlayer()
    {
        
        float time = 0;
        float duration = 3f;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = lastSeenPos;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        StartCoroutine(lookAround());
        state = State.ReturnToStart;
    }

    // kinda works
    IEnumerator lookAround()
    {
        float time = 0.0f;
        float duration = 0.5f;
        float dist = 2;
        Vector2 startPosition = transform.position;
        //look up
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, new Vector2(lastSeenPos.x, lastSeenPos.y + dist), time / duration);
            //canSeePlayer();
            time += Time.deltaTime;
            yield return null;
        }
        // look right
        time = 0;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(lastSeenPos.x + dist, lastSeenPos.y), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        // look down
        time = 0;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(lastSeenPos.x, lastSeenPos.y - dist), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        // look left
        time = 0;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(lastSeenPos.x - dist, lastSeenPos.y), time / duration);
            time += Time.deltaTime;
            yield return null;
        }



    }
}
