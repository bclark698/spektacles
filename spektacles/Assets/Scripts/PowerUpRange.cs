using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpRange : Ability
{
    public GameObject powerUpObj;

    // MAKE SURE ALL THE ELEMENTS ARE IN ORDER OF THE ICON ENUMS!
    // [SerializeField] private Sprite[] desktopIcons = null;

    // [SerializeField] private GameObject powerUpObj; // TODO delete serialize

    void Start(){
        controls.Gameplay.UsePowerUp.started += _ => ButtonHeld();
        controls.Gameplay.UsePowerUp.canceled += _ => ButtonRelease();
    }

    protected override void ButtonHeld() {
        // prevent outlining when no powerup is held
        if(powerUpObj != null) {
            base.ButtonHeld();
        }
    }

    void ButtonRelease() {
        buttonHeld = false;
        UsePowerUp();
    }

    public PowerUp.Type GetHeldPowerUpType() {
        if(powerUpObj == null) {
            return PowerUp.Type.None;
        } else {
            return powerUpObj.GetComponent<PowerUp>().type;
        }
    }

    public void SetNewHeldPowerUp(GameObject newPowerUp) {
        // destroy previously held
        if(powerUpObj != null) {
            Destroy(powerUpObj);
        }
        powerUpObj = newPowerUp;
    }

    /* Handle powerUp. A held powerUp still gets used (wasted) even if no enemies are
     * in range to let player try out using powerups. */
    public void UsePowerUp()
    {
        // Type heldPowerUp = player.heldPowerUp;

        if (powerUpObj != null)
        {
            // get all the enemies within our PowerUpRange
            Collider2D[] enemiesInRange = GetEnemiesInRange();

            // temporarily keep track of the held powerup item because .Use() sets powerUp to None.
            PowerUp.Type powerUpType = powerUpObj.GetComponent<PowerUp>().type; // TODO can we call .Use() after the for loop?

            powerUpObj.GetComponent<PowerUp>().Use();

            // have each enemy determine how to handle this powerup being used on them
            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().HandlePowerUp();
                enemiesInRange[i].GetComponent<Enemy>().OutlineOff();
            }

        }
    }

    /* For testing purposes, this draws red line around the player's power up range.
     * This has no effect during gameplay, so we can leave this in. */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
