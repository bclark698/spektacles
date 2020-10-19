using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource playerHit;
    public AudioSource reloadLvl;
    public AudioSource aquire;
    public AudioSource stoneBlast;
    public AudioSource footstepLoop;
    public AudioSource glassesRecharged;

    /*
    public AudioSource bugSpray;
    public AudioSource zooOom;
    public AudioSource helmetProtect;
    public AudioSource earbudsProtect;
    */


    // Start is called before the first frame update
    void Start()
    {
      footstepLoop.time = .5f;
    }

    public void HitSound(){
      playerHit.Play();
    }

    public void ReloadSound(){
      reloadLvl.Play();
    }

    public void AcquireSound(){
      aquire.Play();
    }

    public void StoneBlastSound(){
      stoneBlast.Play();
    }

    public void FootstepLoopPlay(){
      footstepLoop.Play();
    }

    public void FootstepLoopStop(){
      footstepLoop.Stop();
    }

    public IEnumerator RechargedSound(){
      yield return new WaitForSeconds(3f);
      glassesRecharged.Play();
    //  return null;
    }
}
