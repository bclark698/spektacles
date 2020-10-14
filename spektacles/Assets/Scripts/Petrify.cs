﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Petrify : MonoBehaviour
{
    public PlayerControls controls;

    [SerializeField] private Transform petrifyRangePos;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private float petrifyRange = 10;
    [SerializeField] private float cooldownTime = 3f;

    private Image stoneIcon; // should be the grayscale version of the icon
    public bool coolingDown;
    private float finishCooldownTime;

    private PlayerSoundController playerSounds;

    // Start is called before the first frame update
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Petrify.performed += _ => GetComponent<Player>().LoseGlasses();
    }

    void Start()
    {
        // TODO delete later when implement outline enemies
        // changes power up range indicator to be proper size
        petrifyRangePos.localScale = new Vector3(2 * petrifyRange, 2 * petrifyRange, 0);
        stoneIcon = GameObject.FindGameObjectWithTag("Petrify Icon").GetComponent<Image>();

        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown)
        {
            if(Time.time >= finishCooldownTime) {
                coolingDown = false;
                GetComponent<Player>().PickUpGlasses();
            } else {
                stoneIcon.fillAmount -= 1 / cooldownTime * Time.deltaTime;
            }

        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void PetrifyEnemy()
    {
        if(!coolingDown) {
            playerSounds.StoneBlastSound();
            Debug.Log("petrify");
            coolingDown = true; //set timer to start cooling down
            finishCooldownTime = Time.time + cooldownTime;
            print("cooling down now");

            Camera mainCamera = Camera.main;
            StartCoroutine(mainCamera.GetComponent<CameraInteract>().BeginBlur(cooldownTime));

            // get all the enemies within our PowerUpRange
            Collider2D[] enemiesInRange = GetEnemiesInRange();

            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().TurnIntoStone();
            }

            stoneIcon.fillAmount = 1;
        }
    }

    Collider2D[] GetEnemiesInRange()
    {
        // get all the enemies within our PowerUpRange
        return Physics2D.OverlapCircleAll(petrifyRangePos.position, petrifyRange, whatIsEnemies);
    }
}
