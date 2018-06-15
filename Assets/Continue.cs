using UnityEngine;
using UnityEngine.UI;

public class Continue : MonoBehaviour {

	public GameObject panel;
	private int num = 9;
	public Text countdown_txt;
	private float speed = 0.8f;
    private bool canClick;

	void Start () {
		canClick = false;
		panel.SetActive (false);
		if (Data.Instance.playMode == Data.PlayModes.COMPETITION) {			
			Data.Instance.events.OnGameOver += OnGameOver;
		}
	}
	void Update()
	{
		if (canClick) {
			for (int a = 0; a < 4; a++) {
				if (InputManager.getJump (a))
					OnJoystickClick ();
				if (InputManager.getFireDown (a))
					OnJoystickClick ();
			}
		}
	}
	void OnDestroy()
	{
		Data.Instance.events.OnGameOver -= OnGameOver;
	}
	public void OnGameOver()
	{		
		Invoke ("OnGameOverDelayed", 2);
	}	
	public void OnGameOverDelayed()
	{		
		panel.SetActive (true);
		num = 9;
		countdown_txt.text = num.ToString();
		Invoke ("Loop", 0.5f);
	}	
	public void Loop()
	{
		canClick = true;
		num--;
		if(num<=0)
		{
			canClick = false;
			panel.GetComponent<Animation> ().Play ("signalOff");
			Invoke ("Done", 1f);
			return;
		}
		countdown_txt.text = num.ToString();
		Invoke ("Loop", speed);
	}	
	void Done()
	{
		GetComponent<SummaryCompetitions> ().SetOn ();
		panel.SetActive (false);
	}
	void OnJoystickClick()
	{
		if (canClick) {
			Data.Instance.inputSavedAutomaticPlay.RemoveAllData ();
			Data.Instance.isReplay = true;
			CancelInvoke ();
			Game.Instance.ResetLevel();  
		}
	}

}
