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


    public int cameraFollowSpeed;
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
        Debug.Log("they aint the same");
        StartCoroutine(upatePosition(playerX, playerY));
      }
      Debug.Log(camera.transform.position);
      //Vector3 camUpdate = (camX, camY);
      camera.transform.position = new Vector3(camX, camY, -3);
    }

    IEnumerator upatePosition(float pX, float pY){
      if (pX >= camX && camX <= (pX - .5)){
        camX += (Time.deltaTime * cameraFollowSpeed);
        yield return camX;
      }
      else if (pX < camX){
        camX -= (Time.deltaTime * cameraFollowSpeed);
        yield return camX;
      }
      if (pY >= camY && camY <= (pY - .5)){
        camY += (Time.deltaTime * cameraFollowSpeed);
        yield return camY;
      }
      else if (pY < camY){
        camY -= (Time.deltaTime * cameraFollowSpeed);
        yield return camY;
      }
    }
}
