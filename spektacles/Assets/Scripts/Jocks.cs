﻿using System.Collections;
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
	public float timeToSpot;
    private float yVelo = 0.0f;
    private float xVelo = 0.0f;

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
        float newPosX = Mathf.SmoothDamp(transform.position.x, waypoints[num].transform.position.x, ref xVelo, timeToSpot);
        float newPosY = Mathf.SmoothDamp(transform.position.y, waypoints[num].transform.position.y, ref yVelo, timeToSpot);
        transform.position = new Vector2(newPosX, newPosY);
    }

    public override bool HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("orc handing powerup");
        if(powerUp == jockPowerUp)
        {
            StartCoroutine(HandleStun());
            return true;
        }

        return false;
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
