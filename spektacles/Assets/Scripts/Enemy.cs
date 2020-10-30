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
    [SerializeField] protected float petrifyDuration = 1.5f;

    [SerializeField] protected Animator anim; //TODO delete serial
    private PowerUpRange powerUpRange;

    [SerializeField] private Shader defaultShader;
    [SerializeField] private Shader outlineShader;

    [SerializeField] public Collider2D physicalCollider;

    /* This is used to determine the automatic powerup use case. */
    public bool CanHandlePowerUp() {
      return (powerUpRange.GetHeldPowerUpType() == powerUpToHandle);
    }

    public virtual void HandlePowerUp() {
      OutlineOff();
      if(CanHandlePowerUp())
      {
          StartCoroutine(HandleStun());
      }
    }

    public virtual IEnumerator HandleStun() {
      // mark as stunned for a few seconds
      isStunned = true;

      // wait for stunDuration number of seconds
      yield return new WaitForSeconds(stunDuration);

      isStunned = false;

    }

    /* IMPORTANT: child classes of Enemy should not override this function by having
     * their own Awake() function, or they should call base.Awake() in their's */
    void Awake() {
      spriteRenderer = GetComponent<SpriteRenderer>();
      enemySounds = GameObject.Find("/Unbreakable iPod/Enemy Sounds").GetComponent<EnemySoundController>();
      anim = GetComponent<Animator>();
      powerUpRange = GameObject.FindGameObjectWithTag("PowerUp Range").GetComponent<PowerUpRange>();

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

      // play stone animation
      StartCoroutine(stoneAnimation());

    }

    public IEnumerator stoneAnimation(){
      enemySounds.playTurnStoneSound();
      enemySounds.playStoneCrackSound();
      anim.SetBool("stoned", true);
      Debug.Log("collider off");
      physicalCollider.enabled = false;


      yield return new WaitForSeconds(petrifyDuration);

      anim.SetBool("stoned", false);
      Debug.Log("collider on");
      physicalCollider.enabled = true;
    }

    public void OutlineOn() {
      spriteRenderer.material.shader = outlineShader;
    }

    public void OutlineOff() {
      spriteRenderer.material.shader = defaultShader;
    }
}
