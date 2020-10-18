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
    private Player player;

    private PlayerSoundController playerSounds; //TODO move to ability script?

    /* dont want to override the Awake function in Ability?
    private void Awake()
    {
        controls.Gameplay.Petrify.performed += _ => GetComponent<Player>().LoseGlasses();
    }*/

    void Start()
    {
        //controls.Gameplay.Petrify.performed += _ => GetComponent<Player>().LoseGlasses();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        controls.Gameplay.Petrify.started += _ => ButtonHeld();

        controls.Gameplay.Petrify.canceled += _ => ButtonRelease();
        // TODO delete later when implement outline enemies
        // changes power up range indicator to be proper size
        //petrifyRangePos.localScale = new Vector3(2 * range, 2 * range, 0);

        stoneIcon = GameObject.FindGameObjectWithTag("Petrify Icon").GetComponent<Image>();

        playerSounds = GameObject.Find("/Unbreakable iPod/Player Sounds").GetComponent<PlayerSoundController>();
    }

    void ButtonRelease() {
        Debug.Log("button release");
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
        /*
        if(buttonHeld) {
            enemiesInRange = GetEnemiesInRange();
            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().OutlineOn();
            }
            for (int i = 0; i < enemiesInRange.Length; i++)
            {
                enemiesInRange[i].GetComponent<Enemy>().OutlineOff(); //??? todo how to determine 
            }
        }*/
    }

    public void PetrifyEnemy()
    {
        if(!coolingDown) {
            playerSounds.StoneBlastSound();
            Debug.Log("petrify");
            coolingDown = true; //set timer to start cooling down
            finishCooldownTime = Time.time + cooldownTime;
            print("cooling down now");

            Camera mainCamera = Camera.main; //TODO move this elsewhere?
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
}
