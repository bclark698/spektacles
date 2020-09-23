using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isStunned;

    /* The return value represents whether the powerup used on them was applicable to them. 
     * This is used to determine the automatic powerup use case. */
    public abstract bool HandlePowerUp(PowerUp.PowerUpType powerUp);
    public abstract IEnumerator HandleStun();
}
