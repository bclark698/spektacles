using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixie : Enemy
{
    // different states for enemy
    private enum State
    {
        Waiting,        // waiting to see enemy
        ChaseTarget,    // chasing enemy
        ReturnToStart,  // returning to starting point if loses vision of player
        Stunned         // temporary stop of movement due to a stun powerup
    }

    ///////////////////////
    // PRIVATE VARIABLES //
    ///////////////////////
    private State state;            // current state of enemy (chasing, roaming, etc)
    private Vector3 startingPos;    // pixies' starting position
    private RaycastHit2D hitInfo;   // field of view object
    private GameObject playerObj;   // player object (melita)
    private Transform playerLoc;    // player location (melita)
    private bool playerHit;         // true if player has been hit, false if not
    private PowerUp.PowerUpType pixiePowerUp = PowerUp.PowerUpType.BugSpray;    // bug spray, TODO can this be taken out and put in enemy script?

    ///////////////////////////////////////
    // PUBLIC VARIABLES / UNITY EDITABLE //
    ///////////////////////////////////////
    public float rotationSpeed; // speed at which pixies spin
    public float fovDistance;   // distance away from pixies that they can see
    public float moveSpeed;     // movement speed of pixies

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
        playerObj = GameObject.FindGameObjectWithTag("Player"); // create player object
        playerLoc = playerObj.GetComponent<Transform>(); // find player location

    }

    // Update is called once per frame
    void Update()
    {

        // casts ray starting at transform.pos; casts in direction transform.right; length of ray = fovDistance
        hitInfo = Physics2D.Raycast(transform.position, transform.right, fovDistance);
        
        switch(state)
        {
            default:
            // CASE 1
            // Rotate. If player found, change state to chasing target.
            case State.Waiting:
                rotate();
                /*/
                if (playerSightCheck() == true)
                {
                    state = State.ChaseTarget;
                }  TODO recomment in*/
                break;

            // CASE 2
            // Chase target until line of sight is broken or power-up is used.
            case State.ChaseTarget:
                chaseTarget();
                /*
                if(nonPlayerSightCheck() == true) // CURRENTLY DOESN'T WORK
                {
                    state = State.ReturnToStart;
                }
                if(playerHit == true)
                {
                    state = State.ReturnToStart;
                }*/
                break;

            // CASE 3
            // Return to start if line of sight is broken or power-up is used.
            case State.ReturnToStart:
                returnToStart();
                if(transform.position == startingPos)
                {
                    state = State.Waiting;
                }
                
                break;
        }

    }


    //////////////////////
    // HELPER FUNCTIONS //
    //////////////////////
    
    // check if player is in line of sight
    private bool playerSightCheck()
    {
        bool collisionCheck = false;

        //// must add collider for walls and player
        if (hitInfo.collider != null)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * fovDistance, Color.green);
        }

        if(hitInfo.collider.CompareTag("Player"))
        {
            // player found, need to chase
            collisionCheck = true;
        }
        return collisionCheck;
    }

    // check if pixie loses sight of player
    private bool nonPlayerSightCheck()
    {
        bool collisionCheck = false;

        if (hitInfo.collider.CompareTag("Wall")) //FIX THIS; needs to check for any non player objects (walls, tables, doors)
        {
            // player lost, need to stop chasing
            collisionCheck = true;
        }

        return collisionCheck;
    }

    // check if pixies touches player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHit = true;
        }
    }

    // rotate pixies at rotationSpeed
    private void rotate()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    
    // causes pixies to chase player
    private void chaseTarget()
    {
        // move towards the player using Vector2.MoveTowards(fromPosition, toPosition, speed);
        transform.position = Vector2.MoveTowards(transform.position, playerLoc.position, moveSpeed * Time.deltaTime);
    }

    // sends pixies back to starting position
    private void returnToStart()
    {
        Debug.Log("pixie return to start");
        transform.position = Vector2.MoveTowards(transform.position, startingPos, moveSpeed * Time.deltaTime);
    }

    public override void HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("pixie handing powerup" + powerUp);
        if(powerUp == pixiePowerUp)
        {
            state = State.ReturnToStart;
            returnToStart();
            
        }
        /* //TODO stun not fully implemented yet
         else if(powerUp == PowerUp.Stun) {
            StartCoroutine(HandleStun());
         }
          */
    }

    public override IEnumerator HandleStun()
    {
        // stop movement for a few seconds
        float originalSpeed = moveSpeed;
        float originalRotationSpeed = rotationSpeed;
        State originalState = state;

        moveSpeed = 0;
        rotationSpeed = 0;
        state = State.Stunned;

        // wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        //moveSpeed = originalSpeed;
        rotationSpeed = originalSpeed; //TODO change back
        state = originalState;
    }
}











    
