using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerControl : MonoBehaviour
{
    public AudioMixer mixer;

    public void setMusicLevel(float musicLvl){
      mixer.SetFloat("MusicVol", musicLvl);
   }

   public void setSFXLevel(float sfxLvl){
     mixer.SetFloat("SFXVol", sfxLvl);
   }
}
