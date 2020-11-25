using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour
{
    public GameObject[] vamps;
    public int num = 0;
    private int prevNum;

    public float waitTime;
    private float timer;

    public float stunDuration;
    public bool rand = false;
    public bool go = true;



    void Start() {
        //powerUpToHandle = PowerUp.Type.Garlic;

        timer = waitTime;

        for(int i = 0; i < vamps.Length; i++)
        {
            vamps[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        timer = waitTime;

        if (!rand)
        {
            if (num + 1 == vamps.Length)
            {
                prevNum = num;
                num = 0;
            }
            else
            {
                prevNum = num;
                num++;
            }
        }
        else
        {
            prevNum = num;
            num = Random.Range(0, vamps.Length);
        }

        vamps[prevNum].gameObject.SetActive(false);
        vamps[num].gameObject.SetActive(true);

        HandleStun();

    }


    public  IEnumerator HandleStun()
    {
        yield return new WaitForSeconds(stunDuration);
    }

}




    /*
    public override IEnumerator HandleStun()
    {
        // mark as stunned for a few seconds
        isStunned = true;

        //might be good to add a specific animation for the vampires to play

        // wait for 1.5 seconds
        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }*/

