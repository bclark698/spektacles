using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    public GameObject[] vamps;
    public int num = 0;
    private int prevNum;

    public float waitTime;
    private float timer;

    public bool rand = false;
    public bool go = true;

    public Animator poofAnimator;
    public AudioSource poofSound;

    void Start() {
        //powerUpToHandle = PowerUp.Type.Garlic;

        timer = waitTime;

        for(int i = 0; i < vamps.Length; i++)
        {
            vamps[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        timer = waitTime;

        if (!rand)
        {
            prevNum = num;
            num = (num + 1 == vamps.Length) ? 0 : num+1;
        }
        else
        {
            prevNum = num;
            num = Random.Range(0, vamps.Length);
        }

        StartCoroutine("vampireChange");
    }

    public override void OutlineOn() {
        foreach(GameObject vamp in vamps) {
            vamp.GetComponent<SpriteRenderer>().material.shader = outlineShader;
        }
    }

    public override void OutlineOff() {
        foreach(GameObject vamp in vamps) {
            vamp.GetComponent<SpriteRenderer>().material.shader = defaultShader;
        }
    }

    public IEnumerator vampireChange(){
      poofAnimator.SetTrigger("quickChange");
      poofSound.Play();
      yield return new WaitForSeconds(.25f);
      vamps[prevNum].gameObject.SetActive(false);
      vamps[num].gameObject.SetActive(true);

      yield return null;
    }

}
