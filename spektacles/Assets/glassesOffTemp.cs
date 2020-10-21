using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glassesOffTemp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(){
      gameObject.SetActive(false)
;    }
}
