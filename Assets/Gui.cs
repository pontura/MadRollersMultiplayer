using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gui : MonoBehaviour {
    
    [SerializeField]
    LevelComplete levelComplete;

    public GameObject[] hideOnCompetitions;
	private Data data;   

	private int barWidth = 200;
    private bool MainMenuOpened = false;

    private Events events;
	public MissionIcon missionIcon_to_instantiate;
	[HideInInspector]
	public MissionIcon missionIcon;

	void Start()
	{
		missionIcon = Instantiate (missionIcon_to_instantiate);
		missionIcon.transform.localPosition = new Vector3 (1000, 0, 0);

        events = Data.Instance.events;
        Data.Instance.events.OnMissionComplete += OnMissionComplete;
        Data.Instance.events.OnListenerDispatcher += OnListenerDispatcher;
        Data.Instance.events.OnAvatarCrash += OnAvatarCrash;
        Data.Instance.events.OnAvatarFall += OnAvatarCrash;
    }
    void OnDestroy()
    {
        Data.Instance.events.OnMissionComplete -= OnMissionComplete;
        Data.Instance.events.OnAvatarCrash -= OnAvatarCrash;
        Data.Instance.events.OnAvatarFall -= OnAvatarCrash;
        Data.Instance.events.OnListenerDispatcher -= OnListenerDispatcher;

        events = null;
        levelComplete = null;
    }
    void OnAvatarCrash(CharacterBehavior cb)
    {
        levelComplete.gameObject.SetActive(false); 
    }
    void OnListenerDispatcher(string message)
    {
        levelComplete.gameObject.SetActive(false);  
    }
    void OnMissionComplete(int num)
    {
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION ) return;
        levelComplete.gameObject.SetActive(true);
        levelComplete.Init(num);
    }
    //void OnSetFinalScore(Vector3 pos, int _score)
    //{
    //    scoreLabel.text = _score.ToString();
    //}
    public void Settings()
    {
        Data.Instance.GetComponent<GameMenu>().Init();
    }
}
