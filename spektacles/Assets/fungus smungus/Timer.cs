using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private bool coolingDown;
    [SerializeField] private Text timerText;
    [SerializeField] private Image ability;
    private float elapsed;

    private void Update()
    {
        //currently its own script but could be handled by the player entirely
        if(Input.GetKeyDown(KeyCode.Q) && !coolingDown)
        {
            print("did a thing");
            coolingDown = true;
            timerText.text = "cooling down";
            ability.fillAmount = 1;
        }

        if(coolingDown)
        {
            elapsed += Time.deltaTime;
            ability.fillAmount -= 1 / 3f * Time.deltaTime;
            if(elapsed >= 3f)
            {
                coolingDown = false;
                elapsed = 0;
                timerText.text = "cooldown finished";
            }
        }

    }

}
