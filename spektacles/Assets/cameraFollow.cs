using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    private Camera camera;
    private float camX;
    private float camY;
    private float playerX;
    private float playerY;


    public float cameraFollowSpeed;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
      camera =  gameObject.GetComponent<Camera>();
      playerX = player.transform.position.x;
      playerY = player.transform.position.y;
      camX = camera.transform.position.x;
      camY = camera.transform.position.y;
      camera.transform.position = new Vector3(camX, camY, -3f);
    }

    // Update is called once per frame
    void Update()
    {
      playerX = player.transform.position.x;
      playerY = player.transform.position.y;
      if (playerX != camX || playerY != camY){
        //StartCoroutine(upatePosition(playerX, playerY));

      }
      //Debug.Log(camera.transform.position);
      //Vector3 camUpdate = (camX, camY);
      //camera.transform.position = new Vector3(camX, camY, -3);
      camera.transform.position = Vector3.Slerp(camera.transform.position,new Vector3(playerX, playerY, -3), cameraFollowSpeed * Time.deltaTime);
    }

    IEnumerator upatePosition(float pX, float pY){
      if (pX >= camX && camX <= (pX - .1)){
        camX += (Time.deltaTime * cameraFollowSpeed);

      }
      else if (pX < camX){
        camX -= (Time.deltaTime * cameraFollowSpeed);
        //yield return camX;
      }
      if (pY >= camY && camY <= (pY - .1)){
        camY += (Time.deltaTime * cameraFollowSpeed);
        //yield return camY;
      }
      else if (pY < camY){
        camY -= (Time.deltaTime * cameraFollowSpeed);

      }
        yield return camX;
        yield return camY;
    }
}
