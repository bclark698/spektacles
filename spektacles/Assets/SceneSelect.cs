using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadHome(){
      SceneManager.LoadScene(0);
    }

    public void loadSchool(){
      SceneManager.LoadScene(1);
    }

    public void Quit(){
      Application.Quit();
    }
}
