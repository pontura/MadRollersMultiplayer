using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour {

	public CreditIcon creditIcon;
	public Transform container;
	public GameObject newCreditPanel;

	void Start () {
		Data.Instance.events.AddNewCredit += AddNewCredit;
		newCreditPanel.SetActive (false);
		int totalCredits = Data.Instance.credits;
		for (int a = 0; a < totalCredits; a++) {
			if (a < 6) {
				AddCredit ();
			}
		}
	}
	void OnDestroy()
	{
		Data.Instance.events.AddNewCredit -= AddNewCredit;
	}
	void AddNewCredit()
	{
		Data.Instance.credits++;
		newCreditPanel.SetActive (true);
		AddCredit ();
		StartCoroutine (ClosePanel ());
	}
	IEnumerator ClosePanel()
	{
		yield return new WaitForSeconds (2);
		newCreditPanel.SetActive (false);
	}
	void AddCredit()
	{
		CreditIcon go = Instantiate (creditIcon);
		go.transform.SetParent (container);
		go.transform.localScale = Vector3.one;
	}
	public void RemoveOne()
	{
		CreditIcon[] all = container.GetComponentsInChildren<CreditIcon> ();
		if(all.Length>0)
			all [all.Length - 1].SetOff ();
	}
}
