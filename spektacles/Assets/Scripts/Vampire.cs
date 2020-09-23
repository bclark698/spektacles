using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    private PowerUp.PowerUpType vampirePowerUp = PowerUp.PowerUpType.Garlic;

    public override void HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("vamp handling powerup");
        if (powerUp == vampirePowerUp)
        {
            StartCoroutine(HandleStun());
        }
    }

    public override IEnumerator HandleStun()
    {
        // mark as stunned for a few seconds
        isStunned = true;

        //might be good to add a specific animation for the vampires to play

        // wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        isStunned = false;
    }
}
