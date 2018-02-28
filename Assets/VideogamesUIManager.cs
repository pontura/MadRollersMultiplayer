using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideogamesUIManager : MonoBehaviour {

	public float separation;
	public VideogameButton button_to_instantiate;
	public Transform container;
	public int id = 0;
	public List<VideogameButton> all;

	public void Init (int id) {
		int _id = 0;
		foreach (VideogameData data in Data.Instance.videogamesData.all) {
			VideogameButton button = Instantiate (button_to_instantiate);
			button.transform.SetParent (container);
			button.transform.localPosition = new Vector3 (separation*_id, 0, 0);
			button.Init (data.logo);
			_id++;
			all.Add (button);
		}
		all [id].SetSelected (true);
	}
	public void Select(int id)
	{
		ResetAll ();
		all [id].SetSelected (true);
	}
	void ResetAll()
	{
		foreach (VideogameButton v in all)
			v.SetSelected (false);
	}
	public void Left()
	{
		if (id == all.Count-1)
			return;
		id++;
		ResetAll ();
		all [id].SetSelected (true);
		SelectFirstLevel ();
	}
	public void Right()
	{
		if (id == 0)
			return;
		id--;
		ResetAll ();
		all [id].SetSelected (true);
		SelectFirstLevel ();
	}
	void SelectFirstLevel()
	{
		GetComponent<LevelSelector> ().SelectFirstLevelOf (id);
	}
}
