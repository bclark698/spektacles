using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCooldown : MonoBehaviour
{
    [SerializeField] private float totalCooldownTime = 2f;
    private float currentCooldownTime = 0;

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        Debug.Log("starting damage cooldown");
        var player = GetComponent<Player>();
        player.invincible = true;
        while(currentCooldownTime < totalCooldownTime)
        {
            yield return new WaitForSeconds(1);
            currentCooldownTime += 1;
        }
        currentCooldownTime = 0; //reset
        player.invincible = false;
        Debug.Log("end of damage cooldown");
        yield return null;

    }
}
