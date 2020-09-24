using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : PowerUp
{
    int numUses = 3;

    public override void Use()
    {
        numUses--;
        Debug.Log("Dash numUsesLeft: " + numUses);
        // play a use sound effect (specialized for each powerup?)


        // specialized powerup particle effect?

        // do player dash
        p.Dash();
        
        // destroy powerup gameObject on third use
        if(numUses == 0)
        {
            Destroy(gameObject);
            p.powerUp = PowerUpType.None;
        }
    }
}
