using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPlayer : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(){
      //Debug.Log("hey look! A thing!");
      player.transform.position = new Vector3(6.8f, -19f, 0f);
    }
}
