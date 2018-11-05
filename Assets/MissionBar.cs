using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBar : MonoBehaviour {

	public GameObject panel;
	public ProgressBar progressBar;
	public float totalHits;
	public float value;

	void Start () {
		panel.SetActive (false);
		Data.Instance.events.OnBossInit += OnBossInit;
		Data.Instance.events.OnBossActive += OnBossActive;
		Data.Instance.events.OnBossHitsUpdate += OnBossHitsUpdate;
	}
	void OnDestroy () {
		Data.Instance.events.OnBossInit -= OnBossInit;
		Data.Instance.events.OnBossActive -= OnBossActive;
		Data.Instance.events.OnBossHitsUpdate -= OnBossHitsUpdate;
	}
	void OnBossHitsUpdate(float actualHits)
	{
		progressBar.SetProgression (1-(actualHits / totalHits));
	}
	void OnBossInit (int totalHits) {
		progressBar.SetProgression (1);
		this.totalHits = (float)totalHits;
		panel.SetActive (true);
	}
	void OnBossActive (bool isOn)
	{
		if (!isOn) {
			panel.SetActive (false);
		}
	}
}
