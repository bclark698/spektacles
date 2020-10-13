using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fungus;
using UnityEngine.EventSystems;

public class FungusInteract : MonoBehaviour
{
    public PlayerControls controls;
    public BlockReference blockRef; // block in flowchart to execute on interact
    //public MenuDialog menuDialog;
    //public Selectable selectable;
    //public Flowchart flowchart;

    void Awake()
    {
        controls = new PlayerControls();
        //controls.Gameplay.EquipOrInteract.performed += _ => flowchart.ExecuteBlock("Talk to Mom");
        if (blockRef.block != null)
        {
            controls.Gameplay.EquipOrInteract.performed += _ => blockRef.Execute();
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

    /*
    public void SetMenuDialog(MenuDialog md)
    {
        Debug.Log("menu dialogue set");
        menuDialog = md;
        controls.Gameplay.SelectDialogue.performed += context => ChangeSelectedDialogueOption(context.ReadValue<float>());
    }

    public void SetDefaultSelectable(Selectable s)
    {
        Debug.Log("default selectable set");
        selectable = s;
        controls.Gameplay.SelectDialogue.performed += context => ChangeSelectedDialogueOption(context.ReadValue<float>());
    }

    private void ChangeSelectedDialogueOption(float direction)
    {
        selectable.OnMove();
        if(direction < 0)
        {
            menuDialog.SelectPrevOption();
        } else if(direction > 0)
        {
            menuDialog.SelectNextOption();
        }
    }*/
}
