using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown resolution;

    public void ChangeResolution()
    {
        switch (resolution.value)
        {
            case 0:
                Screen.SetResolution(2560, 1600, false);
                break;
            case 1:
                Screen.SetResolution(2048, 1280, false);
                break;
            case 2:
                Screen.SetResolution(1440, 900, false);
                break;
            case 3:
                Screen.SetResolution(1280, 800, false);
                break;
            case 4:
                Screen.SetResolution(800, 600, false);
                break;
        }
    }
}
