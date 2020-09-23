using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sirens : Enemy
{
    private PowerUp.PowerUpType sirenPowerUp = PowerUp.PowerUpType.EarPlugs;
    public Gravity pull;

    public override bool HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("siren handling powerup " + powerUp);
        if (powerUp == sirenPowerUp)
        {
            StartCoroutine(HandleStun());
            return true;
        }

        return false; // TODO change this to what it should be
    }

    public override IEnumerator HandleStun()
    {
        isStunned = true; //mark as stunned
        pull.inRange = false; //mark player out of range (no continuing to pull)
        pull.gravityRange.enabled = false; //turn off range

        // wait for 5 seconds - long enough to get across range
        yield return new WaitForSeconds(5f);

        pull.gravityRange.enabled = true; //turn on range
        isStunned = false; //no longer stunned
    }

}
