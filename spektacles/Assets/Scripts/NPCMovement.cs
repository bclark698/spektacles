using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;    
    public int num = 0;

    public float minDist;
    public float timeToSpot;
    public float speed;

    public bool rand = false;
    public bool go = true;

    // Update is called once per frame
    void Update()
    {
        if(waypoints.Length > 1) { // > 1 because waypoints includes the parent
            float dist = Vector3.Distance(gameObject.transform.position, waypoints[num].transform.position);

            if (go)
            {
                if (dist > minDist)
                {
                    Move();
                }
                else
                {
                    if (!rand)
                    {
                        if (num + 1 == waypoints.Length)
                        {
                            num = 0;
                            gameObject.transform.position = waypoints[num].transform.position;
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
    }

    public void Move()
    {
        /*
        float newPosX = Mathf.SmoothDamp(transform.position.x, waypoints[num].transform.position.x, ref xVelo, timeToSpot);
        float newPosY = Mathf.SmoothDamp(transform.position.y, waypoints[num].transform.position.y, ref yVelo, timeToSpot);
        transform.position = new Vector2(newPosX, newPosY);
        */
        gameObject.transform.LookAt(waypoints[num].transform.position);
              gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;

              Vector3 dir = waypoints[num].transform.position;
              float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
              gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
      		gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


}
