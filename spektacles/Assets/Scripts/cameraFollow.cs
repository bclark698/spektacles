using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    private Camera cam;

    private float playerX;
    private float playerY;

    [Tooltip("Set how fast the camera follows the player")]
    public float camFollowSpeed;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
      cam =  gameObject.GetComponent<Camera>();
      playerX = player.transform.position.x;
      playerY = player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
      playerX = player.transform.position.x;
      playerY = player.transform.position.y;

      if (cam.transform.position != player.transform.position){
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -3f) , Time.deltaTime * camFollowSpeed);
      }
    }

    public void returnToPlayer(float offsetX, float offsetY)
    {
        //for use in fungus
        player.SetActive(true); //turn player back on
        enabled = true; //follow player again
        player.transform.position = new Vector2(playerX + offsetX, playerY + offsetY); //move player a certain distance away from collider

        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, -3f), Time.deltaTime * camFollowSpeed); //move camera
    }

    public void stopFollow(bool hidePlayer)
    {
        //sometimes i just need the camera to stop following
        //but not for the player to stop/be removed from the scene ya know
        if(hidePlayer)
        {
            player.SetActive(false);
        }
        enabled = false;
    }


}
