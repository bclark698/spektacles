using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isStunned;

    private SpriteRenderer spriteRenderer;
    private EnemySoundController enemySounds;
    [SerializeField] protected PowerUp.Type powerUpToHandle;
    [SerializeField] protected float stunDuration = 1.5f;

    public Animator anim;

    [SerializeField] private Shader defaultShader;
    [SerializeField] private Shader outlineShader;

    /* The bool return value represents whether the powerup used on them was applicable to them.
     * This is used to determine the automatic powerup use case. */
    public virtual bool HandlePowerUp(PowerUp.Type powerUp) {
      if(powerUp == powerUpToHandle)
        {
            StartCoroutine(HandleStun());
            return true;
        }
        return false;
    }

    public virtual IEnumerator HandleStun() {
      // mark as stunned for a few seconds
      isStunned = true;

      // wait for stunDuration number of seconds
      yield return new WaitForSeconds(stunDuration);

      isStunned = false;
    }

    void Start(){
      spriteRenderer = GetComponent<SpriteRenderer>();
      enemySounds = GameObject.Find("/Unbreakable iPod/Enemy Sounds").GetComponent<EnemySoundController>();
      anim = GetComponent<Animator>();

      defaultShader = Shader.Find("Sprites/Default");
      outlineShader = Shader.Find("Sprites/Outline");
      OutlineOff();
    }

    public virtual void TurnIntoStone()
    {
      OutlineOff();
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
      enemySounds.playTurnStoneSound();
      enemySounds.playStoneCrackSound();
      anim.SetBool("stoned", true);

      yield return new WaitForSeconds(1.5f);

      spriteRenderer.color = Color.white;
      anim.SetBool("stoned", false);
    }

    public void OutlineOn() {
      Debug.Log("outline on");
      if(!spriteRenderer) {
        Debug.Log("spriteRenderer null on outline on");
      } else if(!spriteRenderer.material) {
        Debug.Log("material null on outline on");
      } else if(!spriteRenderer.material.shader) {
        Debug.Log("material null on outline on");
      } else if(!outlineShader) {
        Debug.Log("outlineShader null on outline on");
      }
      spriteRenderer.material.shader = outlineShader;
    }

    public void OutlineOff() {
      if(!spriteRenderer) {
        Debug.Log("spriteRenderer null on outline off");
      } else if(!spriteRenderer.material) {
        Debug.Log("material null on outline off");
      } else if(!spriteRenderer.material.shader) {
        Debug.Log("material null on outline off");
      } else if(!defaultShader) {
        Debug.Log("defaultShader null on outline off");
      }
      spriteRenderer.material.shader = defaultShader;
      Debug.Log("outline off");
    }
}
