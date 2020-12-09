using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXTestSound : MonoBehaviour
{

  public GameObject slider;
  public AudioSource testNoise;
    // Start is called before the first frame update
    void Start()
    {
      //slider = gameObject.GetComponent<Slider>();
      //slider.onValueChanged.AddListener (delegate {MouseUpCheck ();});
    }
    public void OnMouseDown(){
    Debug.Log("Mouse Down");
    }

    public void MouseUpCheck(){
      Debug.Log("Mouse up");
      testNoise.Play();
    }
    void OnMouseUp(){
      Debug.Log("Mouse Up");
      testNoise.Play();
    }

    void OnMouseUpAsButton(){
      Debug.Log("Mouse Up as button");
      testNoise.Play();
    }
}
