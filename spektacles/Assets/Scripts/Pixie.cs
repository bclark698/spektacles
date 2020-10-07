using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixie : Enemy
{
    private enum State
    {
        Waiting,        // waiting to see enemy
        ChaseTarget,    // chasing enemy
        Search,         // search for enemy after losing
        ReturnToStart,  // returning to starting point if loses vision of player
        Stunned         // temporary stop of movement due to a stun powerup
    }

    [SerializeField] private State state;            // current state of enemy (chasing, roaming, etc)
    private Vector3 startingPos;    // pixies' starting position
    private GameObject playerObj;   // player object (melita)
    private PowerUp.PowerUpType pixiePowerUp = PowerUp.PowerUpType.BugSpray;    // bug spray, TODO can this be taken out and put in enemy script?
    //private SpriteRenderer spriteRenderer; // used to flip sprite depending on movement direction

    public float rotationSpeed; // speed at which pixies spin
    public float moveSpeed;     // movement speed of pixies
    public AudioSource giggle1; //agro sound
    public AudioSource giggle2; //dissapointed sound
                                //  public Animator anim;

    private FieldOfView fieldOfView;
    [SerializeField] private Transform pfFieldOfView; // a prefab of our field of view. drag this into the pixie's inspector
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 30f;
    private Vector3 lastMoveDir;
    private Vector3[] directions;
    private int directionIdx = 0;
    private float nextActionTime = 0.0f;
    [SerializeField] private float period = 2.5f;

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
        playerObj = GameObject.FindGameObjectWithTag("Player"); // create player object
        //anim = GetComponent<Animator>();

        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);

        // TODO make cleaner
        Vector3 up = new Vector3(0, 1, 0);
        Vector3 down = new Vector3(0, -1, 0);
        Vector3 left = new Vector3(-1, 0, 0);
        Vector3 right = new Vector3(1, 0, 0);
        directions = new Vector3[]{up, right, down, left};

}

    // Update is called once per frame
    void Update()
    {
        // cycle to the next direction to face every period (currently 2.5)
        if (state == State.Waiting && Time.time > nextActionTime)
        {
            if (!PlayerInSight())
            {
                nextActionTime = Time.time + period;

                directionIdx = (directionIdx + 1) % directions.Length;
            }

        }


        if (fieldOfView != null)
        {
            fieldOfView.SetOrigin(transform.position);
            fieldOfView.SetAimDirection(GetAimDir());
        }


        switch (state)
        {
            // CASE 1
            // Rotate. If player found, change state to chasing target.
            default:
            case State.Waiting:
                if (PlayerInSight())
                {
                    state = State.ChaseTarget;
                    giggle1.Play();
                }
                break;

            // CASE 2
            // Chase target until line of sight is broken or power-up is used.
            case State.ChaseTarget:
                ChaseTarget();
                break;

            // CASE 3
            // Return to start if line of sight is broken or power-up is used.
            case State.ReturnToStart:
                StartCoroutine(ReturnHome());
                //ReturnHome();
                giggle2.Play();
                break;


        }
        //anim.SetFloat("lookingAngle", transform.transform.localEulerAngles.z);
        //Debug.Log(transform.transform.localEulerAngles.z);

    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetAimDir()
    {
        if(state == State.Waiting)
        {
            lastMoveDir = directions[directionIdx];
        } else if(state == State.ChaseTarget)
        {
            lastMoveDir = (playerObj.transform.position - transform.position).normalized; //face player
        } else if(state == State.ReturnToStart)
        {
            lastMoveDir = (startingPos - transform.position).normalized; // face starting pos
        }

        return lastMoveDir;
    }

    // using a cone of vision
    private bool PlayerInSight()
    {
        if (Vector3.Distance(transform.position, playerObj.transform.position) < viewDistance)
        {
            // Player inside viewDistance
            Vector3 dirToPlayer = (playerObj.transform.position - GetPosition()).normalized;
            if (Vector3.Angle(GetAimDir(), dirToPlayer) < fov / 2f)
            {
                // Player inside Field of View
                RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToPlayer, viewDistance);
                if (raycastHit2D.collider != null)
                {
                    // Hit something
                    if (raycastHit2D.collider.gameObject.GetComponent<Player>() != null)
                    {
                        // Hit Player
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // causes pixies to chase player
    private void ChaseTarget()
    {
        // move towards the player using Vector2.MoveTowards(fromPosition, toPosition, speed);
        if (!isStunned)
        {
            // causes pixies to continously look towards player
            Vector2 direction = playerObj.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(2, 2, angle);

            transform.position = Vector2.MoveTowards(transform.position, playerObj.transform.position, moveSpeed * Time.deltaTime);

            // lost sight of player
            if (!PlayerInSight())
            {
                state = State.ReturnToStart;
            }
        }
    }

    // check if pixies touches player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            giggle2.Play();
        }
    }

    // sends pixies back to starting position
    IEnumerator ReturnHome()
    {
        Vector3 direction = startingPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(2, 2, angle);
        moveSpeed = 10f;

        float reachedPosDist = 1f;
        while (Vector2.Distance(transform.position, startingPos) > reachedPosDist)
        {
            transform.position = Vector2.MoveTowards(transform.position, startingPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(1, 0, 0);
        state = State.Waiting;
    }

    public override bool HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        //Debug.Log("pixie handling powerup" + powerUp);
        if(powerUp == pixiePowerUp)
        {
            StartCoroutine(HandleStun());
            return true;
        }
        return false;
    }

    public override IEnumerator HandleStun()
    {
        // stop movement for a few seconds
        isStunned = true;
        float originalSpeed = moveSpeed;
        float originalRotationSpeed = rotationSpeed;

        moveSpeed = 0;
        rotationSpeed = 0;
        state = State.Stunned;

        // wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);


        moveSpeed = originalSpeed;
        rotationSpeed = originalRotationSpeed;
        isStunned = false;
        state = State.ReturnToStart;
    }
}
