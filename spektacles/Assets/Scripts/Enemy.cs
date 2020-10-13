using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isStunned;

    private SpriteRenderer spriteRenderer;
    private EnemySoundController enemySounds;

    public Animator anim;



    /* The bool return value represents whether the powerup used on them was applicable to them.
     * This is used to determine the automatic powerup use case. */
    public abstract bool HandlePowerUp(PowerUp.PowerUpType powerUp);

    public abstract IEnumerator HandleStun();

    void Start(){
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      enemySounds = GameObject.Find("/Unbreakable iPod/Enemy Sounds").GetComponent<EnemySoundController>();
        anim = gameObject.GetComponent<Animator>();
    }

    public virtual void TurnIntoStone()
    {
      if (gameObject.GetComponent<SpriteRenderer>() != null){
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      }
      else{
        spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
      }
      enemySounds = GameObject.Find("/Unbreakable iPod/Enemy Sounds").GetComponent<EnemySoundController>();

        Debug.Log("turned into stone");
        StartCoroutine(HandleStun());

        // play grayscale stone animation

        StartCoroutine(stoneAnimation());

    }

    public IEnumerator stoneAnimation(){
      //spriteRenderer.color = Color.gray;
      enemySounds.playTurnStoneSound();
      enemySounds.playStoneCrackSound();
      anim.SetBool("stoned", true);

      yield return new WaitForSeconds(1.5f);

      spriteRenderer.color = Color.white;
        anim.SetBool("stoned", false);
    }
}
