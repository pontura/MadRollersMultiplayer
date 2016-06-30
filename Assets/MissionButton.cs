using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour {

    [SerializeField]
    Stars stars;
    public Image background;
    public Image lockImage;
    public int id;
    public Text missionNum;
    public Text missionDesc;

	// Use this for initialization
	public void Init (int id, string desc) {
        this.id = id;
        missionNum.text = "MISSION " + (id+1).ToString();
        missionDesc.text =  desc.ToUpper();
        int starsQty = Data.Instance.userData.GetStars(id+1);
        stars.Init(starsQty);
	}
    public void OnClick()
    {
        print("OnClick" + lockImage.enabled);
        if (lockImage.enabled) return;
        GameObject.Find("LevelSelector").GetComponent<LevelSelector>().loadLevel(id);
    }
    public void disableButton()
    {
        lockImage.enabled = true;
        missionNum.enabled = false;
        missionDesc.enabled = false;
        stars.gameObject.SetActive(false);
        background.color = background.GetComponent<Button>().colors.disabledColor;
       // gameObject.GetComponent<UIButton>().enabled = false;
       // gameObject.GetComponent<UIButton>().defaultColor = gameObject.GetComponent<UIButton>().disabledColor;
    }
}
