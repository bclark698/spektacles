using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Petrify : MonoBehaviour
{
    public PlayerControls controls;
    [SerializeField]
    private Transform rangePos;
    [SerializeField]
    private LayerMask whatIsEnemies;
    [SerializeField]
    private float range;

    private bool coolingDown;
    [SerializeField] private Image ability;
    private float elapsed;


    // Start is called before the first frame update
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Petrify.performed += _ => PetrifyEnemy();
    }

    void Start()
    {
        rangePos.localScale = new Vector3(2 * range, 2 * range, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (coolingDown)
        {
            elapsed += Time.deltaTime;
            ability.fillAmount -= 1 / 3f * Time.deltaTime;
            if (elapsed >= 3f)
            {
                coolingDown = false;
                elapsed = 0;
                print("cooldown finished");
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
        Debug.Log("petrify");
        // get all the enemies within our PowerUpRange
        Collider2D[] enemiesInRange = GetEnemiesInRange();

        for (int i = 0; i < enemiesInRange.Length; i++)
        {
            Debug.Log("enemiesInRange = " + enemiesInRange.Length);
            enemiesInRange[i].GetComponent<Enemy>().TurnIntoStone();
        }
        coolingDown = true; //set timer to start cooling down
        print("cooling down now");
        ability.fillAmount = 1;
    }

    Collider2D[] GetEnemiesInRange()
    {
        // get all the enemies within our PowerUpRange
        return Physics2D.OverlapCircleAll(rangePos.position, range, whatIsEnemies);
    }
}
