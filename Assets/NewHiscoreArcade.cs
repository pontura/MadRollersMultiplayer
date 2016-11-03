using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewHiscoreArcade : MonoBehaviour {

	public GameObject introPanel;
	public GameObject title;
	public GameObject subtitle;
	public GameObject countDown;
    public GameObject scoreField;

    public Light lightInScene;
	
	
	void Start () {
        scoreField.SetActive(false);
        
        introPanel.SetActive(true);
		Invoke("ResetIntro", 3);

        string actualCompetition = Data.Instance.GetComponent<MultiplayerCompetitionManager>().actualCompetition;

        SetTexts(title, "CAMPEONATO: " + actualCompetition);
	}
	
	void ResetIntro () {
		introPanel.SetActive(false);
		LoopCountDown();
	}

	int sec = 5;
    int n = 0;
    void Update()
    {
        n++;
        if (n > 5)
        {
            lightInScene.intensity = (float)Random.Range(70, 100) / 100;
            n = 0;
        }
    }
    void LoopCountDown () {
		if(sec == 0)
		{
			SetTexts(countDown, "");
			TakePhoto();
		} else
		{
			SetTexts(countDown, sec.ToString());
			Invoke("LoopCountDown", 1.5f);
			sec--;
		}
		
	}
	void TakePhoto()
	{
        Data.Instance.events.OnInterfacesStart();
        scoreField.SetActive(true);
        foreach (Text field in scoreField.GetComponentsInChildren<Text>())
            field.text = Data.Instance.GetComponent<ArcadeRanking>().all[0].score + " PUNTOS";

        GetComponent<WebCamPhotoCamera>().TakePhoto(Data.Instance.GetComponent<ArcadeRanking>().newHiscore);

        Invoke("Reset", 4);
	}
    void Reset()
    {
        Data.Instance.LoadLevel("MainMenuArcade");
    }

    void SetTexts(GameObject container, string _text)
	{
		foreach(Text field in container.GetComponentsInChildren<Text>() )
		{
			field.text = _text;
		}
	}
}
