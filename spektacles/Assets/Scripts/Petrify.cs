using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Petrify : Ability
{
    [SerializeField] private float cooldownTime = 3f;

    private Image stoneIcon; // should be the grayscale version of the icon
    private bool coolingDown;
    private float finishCooldownTime;
    private CameraInteract mainCamera;

    void Start()
    {
        controls.Gameplay.Petrify.started += _ => ButtonHeld();
        controls.Gameplay.Petrify.canceled += _ => ButtonRelease();

        stoneIcon = GameObject.FindGameObjectWithTag("Petrify Icon").GetComponent<Image>();
        mainCamera = Camera.main.GetComponent<CameraInteract>();
    }

    void ButtonRelease() {
        buttonHeld = false;
        player.LoseGlasses();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown)
        {
            if(Time.time >= finishCooldownTime) {
                coolingDown = false;
                player.PickUpGlasses();
            } else {
                stoneIcon.fillAmount -= 1 / cooldownTime * Time.deltaTime;
            }
        }
    }

    public void PetrifyEnemy()
    {
        if(!coolingDown) {
            playerSounds.StoneBlastSound();
            coolingDown = true; //set timer to start cooling down
            finishCooldownTime = Time.time + cooldownTime;

            // Camera mainCamera = Camera.main; //TODO move this elsewhere?
            // StartCoroutine(mainCamera.GetComponent<CameraInteract>().BeginBlur(cooldownTime));
            StartCoroutine(mainCamera.BeginBlur(cooldownTime));
            StartCoroutine(playerSounds.RechargedSound());

            Collider2D[] enemiesInRange = GetEnemiesInRange();

            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().TurnIntoStone();
            }

            stoneIcon.fillAmount = 1;
        }
    }

    /* For testing purposes, this draws red line around the player's power up range.
     * This has no effect during gameplay, so we can leave this in. */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
