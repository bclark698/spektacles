using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    private Camera cam;
    private float camX;
    private float camY;
    private float playerX;
    private float playerY;


    public float camFollowSpeed;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
      cam =  gameObject.GetComponent<Camera>();
      playerX = player.transform.position.x;
      playerY = player.transform.position.y;
      camX = cam.transform.position.x;
      camY = cam.transform.position.y;
      cam.transform.position = new Vector3(camX, camY, -3f);
    }

    // Update is called once per frame
    void Update()
    {
      playerX = player.transform.position.x;
      playerY = player.transform.position.y;
      if (playerX != camX || playerY != camY){
        //StartCoroutine(upatePosition(playerX, playerY));

      }
      //Debug.Log(cam.transform.position);
      //Vector3 camUpdate = (camX, camY);
      //cam.transform.position = new Vector3(camX, camY, -3);
      cam.transform.position = Vector3.Slerp(cam.transform.position,new Vector3(playerX, playerY, -3), camFollowSpeed * Time.deltaTime);
    }

    IEnumerator upatePosition(float pX, float pY){
      if (pX >= camX && camX <= (pX - .1)){
        camX += (Time.deltaTime * camFollowSpeed);

      }
      else if (pX < camX){
        camX -= (Time.deltaTime * camFollowSpeed);
        //yield return camX;
      }
      if (pY >= camY && camY <= (pY - .1)){
        camY += (Time.deltaTime * camFollowSpeed);
        //yield return camY;
      }
      else if (pY < camY){
        camY -= (Time.deltaTime * camFollowSpeed);

      }
        yield return camX;
        yield return camY;
    }
}
