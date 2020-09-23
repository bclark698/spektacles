using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    private PowerUp.PowerUpType vampirePowerUp = PowerUp.PowerUpType.Garlic;

    public override bool HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("vamp handling powerup " + powerUp);
        if (powerUp == vampirePowerUp)
        {
            StartCoroutine(HandleStun());
            return true;
        }
        return false;
    }

    public override IEnumerator HandleStun()
    {
        // mark as stunned for a few seconds
        isStunned = true;

        // wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        isStunned = false;
    }
}
