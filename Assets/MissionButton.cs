using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MissionButton : MonoBehaviour {

	public GameObject missionFields;
	public GameObject missionNames;

	public Text[] overs;

	public Stars stars;
	public Animation anim;
	//public GameObject thumbPanel;

    public Image background;
	public GameObject lockImage;
    public int id;
	public int id_in_videogame;
	public int videoGameID;
	public bool isLocked;

	public Image logo;
	public Image floppyCover;

	public Mission mission;

	// Use this for initialization
	public void Init (Mission mission, int id) {
		this.mission = mission;
        this.id = id;
		VideogameData data = Data.Instance.videogamesData.GetActualVideogameDataByID (mission.videoGameID);

		foreach (Text m in missionFields.GetComponentsInChildren<Text>())
			m.text = data.name + " " + ((int)id+1).ToString() + "/9";

		foreach (Text m in missionNames.GetComponentsInChildren<Text>())
			m.text =  mission.description;
		
        int starsQty = Data.Instance.userData.GetStars(id);
        stars.Init(starsQty);
		background.transform.localEulerAngles = new Vector3 (30, 0, 0);

		logo.sprite = data.logo;
		floppyCover.sprite = data.floppyCover;
	}
    public void OnClick()
    {
		anim.Play ("MissionTopSetActive");
    }
	bool isOn;
	public void SetOn(bool _isOn)
	{
		if (isOn && _isOn)
			return;

		this.isOn = _isOn;
		
		if (_isOn) {
			
			//thumbPanel.SetActive (true);
			anim.Play ("MissionButtonOn");
			foreach (Text m in overs)
				m.color = Color.yellow;
		} else {
			anim.Play ("MissionButtonOff");
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
