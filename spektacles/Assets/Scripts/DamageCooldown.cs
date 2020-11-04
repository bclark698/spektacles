using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCooldown : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 2f;
    private SpriteRenderer sprite;

    public void Start(){
      sprite = GetComponent<SpriteRenderer>();
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer() //TODO: add this to the player script?
    {
        Debug.Log("starting damage cooldown");
        var player = GetComponent<Player>();
        player.invincible = true;
        sprite.color = new Color(0.75f, 0.75f, 0.75f, 1f);

        yield return new WaitForSeconds(cooldownTime);

        player.invincible = false;
        Debug.Log("end of damage cooldown");
        sprite.color = Color.white;
        yield return null;
    }
}
