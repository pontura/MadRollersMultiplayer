using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gui : MonoBehaviour {
    
	public LevelComplete levelComplete;

    public GameObject[] hideOnCompetitions;
	private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

	public MissionIcon missionIcon_to_instantiate;
	[HideInInspector]
	public MissionIcon missionIcon;
	public GameObject killemAll;

	void Start()
	{
		killemAll.SetActive (false);
		missionIcon = Instantiate (missionIcon_to_instantiate);
		missionIcon.transform.localPosition = new Vector3 (1000, 0, 0);

        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
		Data.Instance.events.OnBossActive += OnBossActive;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
		Data.Instance.events.OnBossActive -= OnBossActive;

        levelComplete = null;
    }
	void OnBossActive(bool isOn)
	{
		Reset ();
		if (isOn) {
			killemAll.SetActive (true);
		} else {
			levelComplete.gameObject.SetActive (true);
			levelComplete.Init (Data.Instance.missions.MissionActive.id);
		}
		Invoke ("Reset", 2);
	}
	void Reset()
	{
		levelComplete.gameObject.SetActive(false); 
		killemAll.SetActive (false);
	}
    void OnAvatarCrash(CharacterBehavior cb)
    {
        levelComplete.gameObject.SetActive(false); 
    }
    public void Settings()
    {
        Data.Instance.GetComponent<GameMenu>().Init();
    }
}
