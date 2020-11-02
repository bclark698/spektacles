using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCooldown : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 2f;

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        Debug.Log("starting damage cooldown");
        var player = GetComponent<Player>();
        player.invincible = true;

        yield return new WaitForSeconds(cooldownTime);

        player.invincible = false;
        Debug.Log("end of damage cooldown");
        yield return null;
    }
}
