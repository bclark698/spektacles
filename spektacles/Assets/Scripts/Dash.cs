using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private int numUses = 3;

    // TODO define proper default values
    // [SerializeField] private float dashSpeed = 0; //currently unused
    [SerializeField] private float startDashTime = 0;
    [SerializeField] private float dashTime = 0;
    private Player player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        dashTime = startDashTime;
    }

    public void Use()
    {
        numUses--;
        Debug.Log("Dash numUsesLeft: " + numUses);

        // do player dash
        PerformDash();
        
        // destroy powerup gameObject on third use
        if(numUses == 0)
        {
            Destroy(gameObject);
            player.heldPowerUp = PowerUp.Type.None;
        }
    }

    // makes melita zoom zoom
    public void PerformDash()
    {
        /*
        Debug.Log("zoom?");
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (dashTime <= 0)
        {
            dashTime = startDashTime;
            movementVelocity = Vector2.zero;
        }
        else
        {
            dashTime -= Time.deltaTime;
            movementVelocity = direction.normalized * dashSpeed;
            Debug.Log("dashSpeed = " + dashSpeed);
            Debug.Log("Movement Velocity = " + movementVelocity);
            FixedUpdate();
        }*/

    }
}
