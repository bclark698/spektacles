using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    private PowerUp.PowerUpType vampirePowerUp = PowerUp.PowerUpType.Garlic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandlePowerUp(PowerUp.PowerUpType powerUp)
    {
        Debug.Log("siren handling powerup " + powerUp);
        if (powerUp == vampirePowerUp)
        {
            StartCoroutine(HandleStun());
        }
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
