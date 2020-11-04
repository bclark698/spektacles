using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
    public static bool allowAbility = true;
	protected PlayerSoundController playerSounds;
	public PlayerControls controls;
    private LayerMask enemyLayerMask;
    [SerializeField] protected float range = 10;
	protected bool buttonHeld;
	protected Player player;


	void Awake() {
		controls = new PlayerControls();
        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();
		enemyLayerMask = LayerMask.GetMask("Enemy");
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	protected virtual void ButtonHeld() {
        if(allowAbility) {
            buttonHeld = true;

            // fix bug where enemies already in range when beginning button hold are not outlined
            Collider2D[] enemiesInRange = GetEnemiesInRange();

            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().OutlineOn();
            }
        }
    }

    protected virtual void ButtonRelease() {
        if(allowAbility) {
            buttonHeld = false;

            // fix bug where enemies already in range at moment of button release are not un-outlined
            Collider2D[] enemiesInRange = GetEnemiesInRange();

            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().OutlineOff();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		return;
    	}
        if(buttonHeld && other.CompareTag("Enemy")) {
            other.gameObject.GetComponent<Enemy>().OutlineOn();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		return;
    	}
        if(buttonHeld && other.CompareTag("Enemy")) {
            other.gameObject.GetComponent<Enemy>().OutlineOff();
        }
    }

    public Collider2D[] GetEnemiesInRange()
    {
        // get all the enemies within our PowerUpRange
        return Physics2D.OverlapCircleAll(transform.position, range, enemyLayerMask);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
