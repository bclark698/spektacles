using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class MixerControl : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] private TextMeshProUGUI musicPercentageDisplay = null;
    [SerializeField] private TextMeshProUGUI sfxPercentageDisplay = null;

    void Awake() {
      musicPercentageDisplay.text = "100%";
      sfxPercentageDisplay.text = "100%";
    }

    public void setMusicLevel(float musicLvl){
      SetLevel("MusicVol", musicLvl, musicPercentageDisplay);
    }

    public void setSFXLevel(float sfxLvl){
      SetLevel("SFXVol", sfxLvl, sfxPercentageDisplay);
    }

    // make the decibel changes logarithmic instead of linear (too drastic of changes)
    private float ConvertVolume(float sliderVal) {
      return Mathf.Log10(sliderVal) * 20;
    }

    private void SetLevel(string audioChannel, float sliderVal, TextMeshProUGUI percentageDisplay) {
      float convertedVol = ConvertVolume(sliderVal);
      mixer.SetFloat(audioChannel, convertedVol);
      percentageDisplay.text = Mathf.Round(sliderVal*100)+"%"; // TODO round to whole number>
    }
}
