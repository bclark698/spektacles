using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jocks : Enemy
{
    //private enum State { Stunned, Patroling }; // are these states needed?
   // private State state = State.Patroling;
    private PowerUp.PowerUpType jockPowerUp = PowerUp.PowerUpType.Helmet;
    public GameObject[] waypoints;
	public int num = 0;

	public float minDist;
	public float speed;

	public bool rand = false;
	public bool go = true;


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
                if(!rand)
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

        gameObject.transform.LookAt(waypoints[num].transform.position);
        gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;

        Vector3 dir = waypoints[num].transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override bool HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("orc handing powerup");
        if(powerUp == jockPowerUp)
        {
            StartCoroutine(HandleStun());
        }

        return false; // TODO change this to what it should be
    }

    public override IEnumerator HandleStun()
    {
        Debug.Log("orc stunned");
        go = false;
        isStunned = true;

        yield return new WaitForSeconds(1.5f);

        Debug.Log("orc no longer stunned");
        go = true;
        isStunned = false;
    }

}
