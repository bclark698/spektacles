using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInteractIndicator : MonoBehaviour
{
    public GameObject InteractIndicator;

    // Start is called before the first frame update
    void Start()
    {
        InteractIndicator = GameObject.FindGameObjectWithTag("Interact");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // show interact indicator
            InteractIndicator.GetComponent<TextMeshProUGUI>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractIndicator.GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }
}
