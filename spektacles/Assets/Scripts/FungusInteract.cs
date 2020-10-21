using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fungus;
using UnityEngine.EventSystems;

public class FungusInteract : MonoBehaviour
{
    public PlayerControls controls;
    [SerializeField] private BlockReference blockRef; // block in flowchart to execute on interact
    [SerializeField] private string targetTag = "Player";
    private bool targetInRange;

    private musicController musicSounds;

    void Awake()
    {
        controls = new PlayerControls();
        if (blockRef.block != null)
        {
            controls.Gameplay.EquipOrInteract.performed += _ => ExecuteBlock();
        }
        musicSounds = GameObject.Find("/Unbreakable iPod").GetComponent<musicController>();
    }

    void ExecuteBlock() {
        musicSounds.loadCustceneMusic();
        Debug.Log("Block executing");
        if(targetInRange) {
            blockRef.Execute();
        }

    }

    // Called when the Player object is enabled
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(targetTag)) {
            targetInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag(targetTag)) {
            targetInRange = false;
        }
    }
}
