﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JournalIntroduction : MonoBehaviour
{
	// [SerializeField] private PauseMenu PauseMenuInstance;
    void Start() {
    	UnityEvent attentionCleared = GetComponent<Attention>().attentionCleared;
    	attentionCleared.AddListener(IntroduceJournal);
    }

    void IntroduceJournal() {
    	PauseMenu.instance.PauseOrResume();
    }
}
