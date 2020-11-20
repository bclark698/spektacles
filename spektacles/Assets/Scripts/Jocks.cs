using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jocks : Enemy
{
    //private enum State { Stunned, Patroling }; // are these states needed?
   // private State state = State.Patroling;
    public GameObject[] waypoints;
	public int num = 0;
    public float waitTime;
    private float timer;

    public float minDist;
	public float timeToSpot;
    private float yVelo = 0.0f;
    private float xVelo = 0.0f;

	public bool rand = false;
	public bool go = true;

    private JorcSoundController jorcSounds;

    void Start() {
        powerUpToHandle = PowerUp.Type.Helmet;

        timer = waitTime;

        jorcSounds = gameObject.GetComponent<JorcSoundController>();
        jorcSounds.footstepLoopPlay();
    }


    private void Update()
    {
        float dist = Vector3.Distance(gameObject.transform.position, waypoints[num].transform.position);

        if(go)
        {
            if(dist > minDist)
            {
                Move();
            }
            else
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    return;
                }
                timer = waitTime;

                if (!rand)
                {
                    if(num + 1 == waypoints.Length)
                    {
                        num = 0;
                    }
                    else
                    {
                        num++;
                    }
                }
                else
                {
                    num = Random.Range(0, waypoints.Length);
                }
            }
        }
    }

    public void Move()
    {
        float newPosX = Mathf.SmoothDamp(transform.position.x, waypoints[num].transform.position.x, ref xVelo, timeToSpot);
        float newPosY = Mathf.SmoothDamp(transform.position.y, waypoints[num].transform.position.y, ref yVelo, timeToSpot);
        transform.position = new Vector2(newPosX, newPosY);
    }

    public void Wait()
    {

    }

    public override IEnumerator HandleStun()
    {
        go = false;
        isStunned = true;

        yield return new WaitForSeconds(stunDuration);

        go = true;
        isStunned = false;

    }

}
