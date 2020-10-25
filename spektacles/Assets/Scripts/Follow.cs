using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    private Camera cam;

    private float targetX;
    private float targetY;

    [Tooltip("Set how fast the camera follows the target")]
    [SerializeField] private float followSpeed = 20f;
    private GameObject target;
    [SerializeField] private string targetTag = "Player"; // default value to follow the player


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        cam = gameObject.GetComponent<Camera>();
        targetX = target.transform.position.x;
        targetY = target.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = target.transform.position.x;
        targetY = target.transform.position.y;

        if (cam.transform.position != target.transform.position)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position , new Vector3(targetX, targetY + 5, -3f), Time.deltaTime * followSpeed);
        }
    }

    public void returnToPlayer(float offsetX, float offsetY)
    {
        //for use in fungus
        target.SetActive(true); //turn target back on
        enabled = true; //follow target again
        target.transform.position = new Vector2(targetX + offsetX, targetY + offsetY); //move target a certain distance away from collider
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(target.transform.position.x, target.transform.position.y, -3f), Time.deltaTime * followSpeed); //move camera
    }

    public void stopFollow(bool hideTarget)
    {
        //sometimes i just need the camera to stop following
        //but not for the target to stop/be removed from the scene ya know
        if (hideTarget)
        {
            target.SetActive(false);
        }
        enabled = false;
    }


}
