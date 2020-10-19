using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    void Start() {
        powerUpToHandle = PowerUp.Type.Garlic;
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
}
