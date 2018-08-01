using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour {

	public GameObject creditIcon;
	public Transform container;
	public GameObject newCreditPanel;

	void Start () {
		newCreditPanel.SetActive (false);
		for (int a = 0; a < Data.Instance.credits; a++) 
			AddCredit ();
		
	}
	public void AddNewCredit()
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
		GameObject go = Instantiate (creditIcon);
		go.transform.SetParent (container);
		go.transform.localScale = Vector3.one;
	}
	public void RemoveOne()
	{
		Transform[] all = container.GetComponentsInChildren<Transform> ();
		Destroy (all [all.Length - 1].gameObject);
	}
}
