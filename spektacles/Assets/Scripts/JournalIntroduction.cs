using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class JournalIntroduction : MonoBehaviour
{
	// [SerializeField] private PauseMenu PauseMenuInstance;
    void Start() {
    	UnityEvent attentionCleared = GetComponent<Attention>().attentionCleared;
    	attentionCleared.AddListener(IntroduceJournal);

    	// set first initial objective in home
    	TextMeshProUGUI objectiveText = GameObject.FindGameObjectWithTag("Objective Text").GetComponent<TextMeshProUGUI>();
		objectiveText.text = "Check out what's downstairs";
    }

    void IntroduceJournal() {
    	PauseMenu.instance.Pause();
    	Debug.Log("INTRODUCE JOURNAL");
    }
}
