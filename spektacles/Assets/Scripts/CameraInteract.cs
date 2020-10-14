using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteract : MonoBehaviour
{
	BoxBlur boxBlur;
    // Start is called before the first frame update
    void Start()
    {
        boxBlur = GetComponent<BoxBlur>();
        boxBlur.enabled = false;
    }

    // duration is in seconds. represents how long the blur will last
    public IEnumerator BeginBlur(float duration) {
        // re-enable blur
        boxBlur.enabled = true;
        
        // wait for duration number of seconds
        yield return new WaitForSeconds(duration);

        boxBlur.enabled = false;
    }
}
