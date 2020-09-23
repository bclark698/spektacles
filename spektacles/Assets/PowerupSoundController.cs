using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSoundController : MonoBehaviour
{
    public AudioSource pickUp;
    public AudioSource equip;
    public AudioSource bugSpray;
    public AudioSource helmetBlock;
    public AudioSource garlicBlock;
    public AudioSource earbudsBlock;
    public AudioSource glassesBuff;
    public AudioSource zoom;

    // Start is called before the first frame update
    void Start()
    {

    }
        public void pickUpSound(){
          pickUp.Play();
        }

        public void equipSound(){
          equip.Play();
        }

        public void bugSpraySound(){
          bugSpray.Play();
        }

        public void helmetBlockSound(){
          helmetBlock.Play();
        }

        public void garlicBlockSound(){
          garlicBlock.Play();
        }

        public void earbudsBlockSoundStart(){
          earbudsBlock.Play();
        }

        public void earbudsBlockSoundStop(){
          earbudsBlock.Stop();
        }

        public void glassBuffBlockSound(){
          glassesBuff.Play();
        }

        public void zoomSound(){
          zoom.Play();
        }

}
