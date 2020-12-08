using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class MixerControl : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] private TextMeshProUGUI musicPercentage;
    [SerializeField] private TextMeshProUGUI sfxPercentage;

    public void setMusicLevel(float musicLvl){
      mixer.SetFloat("MusicVol", musicLvl);
      Debug.Log("MUSIC LVL: "+musicLvl); // TODO remove- used for debugging
      musicPercentage.text = "100%"; // TODO actually calculate
   }

   public void setSFXLevel(float sfxLvl){
     mixer.SetFloat("SFXVol", sfxLvl);
     Debug.Log("SFX LVL: "+sfxLvl); // TODO remove- used for debugging
     sfxPercentage.text = "100%"; // TODO actually calculate
   }
}
