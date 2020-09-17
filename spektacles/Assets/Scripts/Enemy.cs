using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void HandlePowerUp(PowerUp.PowerUpType powerUp);
    public abstract IEnumerator HandleStun();
}
