using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInteractIndicator : MonoBehaviour
{
    [SerializeField]
    public GameObject InteractIndicator;

    // Start is called before the first frame update
    void Start()
    {
      //  InteractIndicator = GameObject.FindGameObjectWithTag("Interact");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // show interact indicator
            InteractIndicator.GetComponent<TextMeshProUGUI>().enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // unshow interact indicator
            InteractIndicator.GetComponent<TextMeshProUGUI>().enabled = false;
        }
    }
}
