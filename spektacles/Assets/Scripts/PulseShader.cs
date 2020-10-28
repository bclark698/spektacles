using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseShader : MonoBehaviour
{
	[SerializeField] private float increasePerSec = 0.5f;
	[SerializeField] private float maxOpacity = 0.7f;
    private float opacity = 0;
    private bool increase = true;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // increase and decrease blur over time
        float delta = Time.deltaTime * increasePerSec;
        if (opacity + delta >= maxOpacity) {
            increase = false;
        } else if(opacity <= 0) {
            increase = true;
        }

        if (increase) {
            opacity += delta;
        } else {
            opacity -= delta;
        }
        Color temp = image.color;
     	temp.a = opacity;
     	image.color = temp;
        // image.color.a = opacity;
    }
}
