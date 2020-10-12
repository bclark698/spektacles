using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fungus;

public class FungusInteract : MonoBehaviour
{
    public PlayerControls controls;
    public BlockReference blockRef;
    //public Flowchart flowchart;

    void Awake()
    {
        controls = new PlayerControls();
        //controls.Gameplay.EquipOrInteract.performed += _ => flowchart.ExecuteBlock("Talk to Mom");
        controls.Gameplay.EquipOrInteract.performed += _ => blockRef.Execute();
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
}
