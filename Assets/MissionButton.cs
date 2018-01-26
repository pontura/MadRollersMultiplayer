using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour {

	public GameObject missionFields;
	public GameObject missionNames;

	public Text[] overs;

	public Stars stars;

    public Image background;
	public GameObject lockImage;
    public int id;
	public bool isLocked;

	// Use this for initialization
	public void Init (int id, string desc) {
        this.id = id;

		foreach (Text m in missionFields.GetComponentsInChildren<Text>())
			m.text = "MISSION " + (id+1).ToString();

		foreach (Text m in missionNames.GetComponentsInChildren<Text>())
			m.text =  desc.ToUpper();
		
        int starsQty = Data.Instance.userData.GetStars(id);
        stars.Init(starsQty);
		background.transform.localEulerAngles = new Vector3 (30, 0, 0);
	}
    public void OnClick()
    {
		//if (isLocked) return;
       // GameObject.Find("LevelSelector").GetComponent<LevelSelector>().loadLevel(id);
    }
	public void SetOn(bool isOn)
	{
		if (isOn) {
			foreach (Text m in overs)
				m.color = Color.yellow;
		} else {
			foreach (Text m in overs)
			{
				if (isLocked) {
					m.color = Color.red;
				} else {
					m.color = Color.white;
				}
			}
		}
	}
    public void disableButton()
    {
		isLocked = true;
		lockImage.SetActive(true);
        stars.gameObject.SetActive(false);
		foreach (Text m in overs)
			m.color = Color.red;
    }
}
