using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : MonoBehaviour
{

    private GameObject tipObject;

    // Start is called before the first frame update
    void Start()
    {
      //  tipObject = GameObject.Find("Tip");
      tipObject = gameObject.transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter2D(Collider2D col){
      if (col.gameObject.CompareTag("Player"))
       {
            tipObject.SetActive(true);
       }
    }


      void OnTriggerExit2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
       {
            tipObject.SetActive(false);
       }

    }
}
