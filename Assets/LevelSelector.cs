using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelector : MonoBehaviour {

    public int levelUnlockedID;
    public MissionButton uiButton;

    [SerializeField]
    GameObject container;

	private Data data;
	
	private Missions missions;
	private int missionID;
    private float StartScrollPosition;
    private MissionButton lastButton;
    public bool showJoystickMenu;


	void Start () {

        Data.Instance.events.VoiceFromResources("juega_solo_asi_te_quedaras");
        Data.Instance.events.OnInterfacesStart();

		data = GameObject.Find("data").GetComponent<Data>();		
		missions = GameObject.Find("data").GetComponent<Missions>();


       // GameObject container = GameObject.Find("Container") as GameObject;

		missionID = 0;

        //si jugas con joystick
        //if (!showJoystickMenu)
        //    GameObject.Find("ContainerJoystick").gameObject.SetActive(false);


		foreach(Mission mission in missions.missions)
		{
            //si jugas con joystick
            if (showJoystickMenu) return;

            MissionButton button = Instantiate(uiButton) as MissionButton;
            
            //UIButtonKeys keys = button.GetComponent<UIButtonKeys>();

            button.transform.SetParent(container.transform) ;
            button.transform.localScale = new Vector3(1,1,1);
            button.transform.eulerAngles = new Vector3(0, 0, Random.Range(-3,3));

            if (missionID > data.levelUnlockedID && !Data.Instance.DEBUG)
            {
                button.disableButton();
            }
            button.Init(missionID, mission.description.ToUpper());

            missionID++;
            lastButton = button;
            
       
		}
        levelUnlockedID = data.levelUnlockedID;        
	}
    public void loadLevel(int num)
    {
        data.missionActive = num+1;
        Data.Instance.LoadLevel("Game");
    }
    public void Back()
    {
        Data.Instance.LoadLevel("MainMenu");
    }
}
