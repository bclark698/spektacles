using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability : MonoBehaviour
{
	public PlayerControls controls;
	// [SerializeField] private Transform transform; // TODO remove serial
    [SerializeField] private LayerMask enemyLayerMask; // TODO remove serial
    [SerializeField] private float range = 10;
	[SerializeField] protected bool buttonHeld; // TODO remove serialize field- used for testing outline

	void Awake() {
		controls = new PlayerControls();
		// transform = GetComponent<Transform>();
		enemyLayerMask = LayerMask.GetMask("Enemy");
	}

	protected virtual void ButtonHeld() {
        Debug.Log("button held started");
        buttonHeld = true;

        // fix bug where enemies already in range when beginning button hold are not outlined
        Collider2D[] enemiesInRange = GetEnemiesInRange();

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            enemiesInRange[i].GetComponent<Enemy>().OutlineOn();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		return;
    	}
        if(buttonHeld && other.CompareTag("Enemy")) {
        	Debug.Log("on trigger outline on");
            other.gameObject.GetComponent<Enemy>().OutlineOn();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
    	if(other.CompareTag("Player")) {
    		return;
    	}
        if(buttonHeld && other.CompareTag("Enemy")) {
        	Debug.Log("on trigger outline off");
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

    /* For testing purposes, this draws red line around the player's power up range.
     * This has no effect during gameplay, so we can leave this in. */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
