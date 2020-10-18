using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public bool isStunned;

    private SpriteRenderer spriteRenderer;
    private EnemySoundController enemySounds;

    public Animator anim;

    [SerializeField] private Shader defaultShader;
    [SerializeField] private Shader outlineShader;

    /* The bool return value represents whether the powerup used on them was applicable to them.
     * This is used to determine the automatic powerup use case. */
    public abstract bool HandlePowerUp(PowerUp.PowerUpType powerUp);

    public abstract IEnumerator HandleStun();

    void Start(){
      spriteRenderer = GetComponent<SpriteRenderer>();
      enemySounds = GameObject.Find("/Unbreakable iPod/Enemy Sounds").GetComponent<EnemySoundController>();
      // anim = gameObject.GetComponent<Animator>();
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
      //spriteRenderer.color = Color.gray;
      enemySounds.playTurnStoneSound();
      enemySounds.playStoneCrackSound();
      anim.SetBool("stoned", true);

      yield return new WaitForSeconds(1.5f);

      spriteRenderer.color = Color.white;
      anim.SetBool("stoned", false);
    }

    public void OutlineOn() {
      Debug.Log("outline on");
      spriteRenderer.material.shader = outlineShader;
      // spriteRenderer.material = outlineMaterial;
      // spriteRenderer.material.SetInt("_OutlineEnabled", 1);
    }

    public void OutlineOff() {
      spriteRenderer.material.shader = defaultShader;
      Debug.Log("outline off");
      // spriteRenderer.material = defaultMaterial;
      // spriteRenderer.material.SetInt("_OutlineEnabled", 0);
    }
}
