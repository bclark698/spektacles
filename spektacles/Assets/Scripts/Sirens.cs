using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sirens : Enemy
{
    public Gravity pull;

    void Start() {
        powerUpToHandle = PowerUp.Type.EarPlugs;
    }

    public override IEnumerator HandleStun()
    {
        isStunned = true; //mark as stunned
        pull.inRange = false; //mark player out of range (no continuing to pull)
        pull.gravityRange.enabled = false; //turn off range

        // wait for 5 seconds - long enough to get across range
        yield return new WaitForSeconds(stunDuration);

        pull.gravityRange.enabled = true; //turn on range
        isStunned = false; //no longer stunned
    }
}
