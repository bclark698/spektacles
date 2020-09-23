using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isStunned;
    public abstract bool HandlePowerUp(PowerUp.PowerUpType powerUp);
    public abstract IEnumerator HandleStun();
}
